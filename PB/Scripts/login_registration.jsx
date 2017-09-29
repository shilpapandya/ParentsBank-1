//var RegLog = React.createClass({

class RegLog extends React.Component{

    constructor(props) {
        super(props);

        this.state = {
            login: true,
            reg: false,
            login_username: '',
            login_password: ''
        };

        this.handleLoginClick = this.handleLoginClick.bind(this);
        this.handleRegClick = this.handleRegClick.bind(this);
        this.setLoginFields = this.setLoginFields.bind(this);
        this.LogUserIn = this.LogUserIn.bind(this);
        //this.setLoginUserName = this.setLoginUserName.bind(this);
    }

    handleLoginClick() {
        this.setState({
            login: true,
            reg: false           
        })
      /*  if (this.state.login) {
            jQuery("#login_form").show();
        } else if (this.state.reg) {
            jQuery("#register_form").show();
        }    */   
    }
    handleRegClick() {
        this.setState({
            login: false,
            reg: true
        })
    }

    setLoginFields(event) {

        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        console.log("Target ", target);
        console.log("value " ,value);
        console.log("name " ,name);

        this.setState({
            [name]: value
        });

        /* this.setState({
            login_username: e.target.value
        }) */

        console.log(this.state);
    }

    LogUserIn(event) {
        //alert('An essay was submitted: ' + this.state)
        event.preventDefault();
        console.log(this.state)

        
        var url = '@Url.Action("LoginViewModel","Account/Login")';
        var url1 = '/Account/Login';

        var model = { Email: this.state.login_username, Password: this.state.login_password };

         jQuery.ajax({
            type: "POST",
            url: url1,
            data: JSON.stringify({ model: model }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) { console.log(data) },
            error: function (err, txt) { console.log(err); console.log(txt) }
        }); 

        return false;
    }

   

    //render: function () {
    render() {
        return (
             
            <div className="modal fade" id="myModal" tabIndex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>         
                        </div>
                        <div className="container">
                            <div className="row">
                                <div className="col-md-6 col-md-offset-3">
                                    <div className="panel panel-login">
                                        <div className="panel-heading">
                                            <div className="row">
                                                <div className="col-xs-6">
                                                    <a href="#" className={this.state.login ? 'active' : null} id="login_form_link" onClick={this.handleLoginClick}>Login</a>
                                                </div>
                                                <div className="col-xs-6">
                                                    <a href="#" id="register_form_link" className={this.state.reg ? 'active' : null} onClick={this.handleRegClick}>Register</a>
                                                </div>
                                            </div>
                                            <hr />


                                            <div className="panel-body">
                                                <div className="row">
                                                    <div className="col-lg-12">

                                                        <form id="login_form" method="post" role="form" className={this.state.login ? 'displayBlock' : 'hide'}>
                                                            <div className="form-group">
                                                                <input type="email" name="login_username" id="username" tabIndex="1" className="form-control" placeholder="Email" value={this.state.login_username} onChange={this.setLoginFields} /> 
                                                            </div>
                                                            <div className="form-group">
                                                                <input type="password" name="login_password" id="password" tabIndex="2" className="form-control" placeholder="Password" value={this.state.login_password} onChange={this.setLoginFields} />
                                                            </div>
                                                            
                                                            <div className="form-group">
                                                                <div className="row">
                                                                    <div className="col-sm-6 col-sm-offset-3">
                                                                        <button type="submit" name="login-submit" id="login-submit" tabIndex="4" className="form-control btn btn-login btn-primary"  onClick={this.LogUserIn} >Log In</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div className="form-group">
                                                                <div className="row">
                                                                    <div className="col-lg-12">
                                                                        <div className="text-center">
                                                                            <a href="" tabIndex="5" className="forgot-password">Forgot Password?</a>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                    </form>



                                                        <form id="register_form" method="post" role="form" className={this.state.reg ? 'displayBlock' : 'hide'}>
                                                            <div className="form-group">
                                                                <input type="text" name="username" id="username" tabIndex="1" className="form-control" placeholder="Username" value="" />
                                                            </div>
                                                            <div className="form-group">
                                                                <input type="email" name="email" id="email" tabIndex="1" className="form-control" placeholder="Email Address" value="" />
                                                            </div>
                                                            <div className="form-group">
                                                                <input type="password" name="password" id="password" tabIndex="2" className="form-control" placeholder="Password" />
                                                            </div>
                                                            <div className="form-group">
                                                                <input type="password" name="confirm-password" id="confirm-password" tabIndex="2" className="form-control" placeholder="Confirm Password" />
                                                            </div>
                                                            <div className="form-group">
                                                                <div className="row">
                                                                    <div className="col-sm-6 col-sm-offset-3">
                                                                        <input type="submit" name="register-submit" id="register-submit" tabIndex="4" className=" btn btn-register btn-primary" value="Register Now" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </form>




                                                    </div>
                                                </div>
                                            </div>



                                        </div>     
                                    </div>
                                </div>
                            </div>
                        </div>    
                        
                    </div>
                </div >
            </div >

        );
    }
} //);
ReactDOM.render(
    <RegLog />,
    document.getElementById('reglog')
);