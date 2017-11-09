$(document).ready(function () {

    $('#myCarousel').carousel({
        interval: 4000
    });

    var clickEvent = false;
    $('#myCarousel').on('click', '.nav a', function () {
        clickEvent = true;
        $('.nav li').removeClass('active');
        $(this).parent().addClass('active');
    }).on('slid.bs.carousel', function (e) {
        if (!clickEvent) {
            var count = $('.nav-pills').children().length - 1;
            var current = $('.nav-pills li.active');
            current.removeClass('active').next().addClass('active');
            var id = parseInt(current.data('slide-to'));
            if (count == id) {
                $('.nav-pills li').first().addClass('active');
            }
        }
        clickEvent = false;
        });






    barChart();
    $(window).resize(function () {
        barChart();
    });

    function barChart() {
        
        $('.bar-chart').find('.item-progress').each(function () {
            var itemProgress = $(this),
                interestAccured = $("#interest_accured").text(),
                totalAmt = itemProgress.data('percent'),
                principal = totalAmt - interestAccured,
                itemProgressWidth = $(this).parent().width() * (principal / totalAmt);
            itemProgress.css('width', itemProgressWidth);

            console.log($(this).parent().width());
            console.log(itemProgressWidth);
                console.log(" interest Accured " + interestAccured + " : " + totalAmt + " : " + principal)

        });
    }


})