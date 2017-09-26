var CommentBox = React.createClass({
    render: function () {
        return (
            <div className="commentBox">
                Hello, world! I am a CommentBox.
                <h1>Now Using React!!</h1>
            </div>
        );
    }
});
ReactDOM.render(
    <CommentBox />,
    document.getElementById('tut')
);