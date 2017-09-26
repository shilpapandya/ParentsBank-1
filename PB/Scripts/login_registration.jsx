//var RegLog = React.createClass({

class RegLog extends React.Component{

    constructor(props) {
        super(props);
        this.state = { login: true, reg: false };
        this.handleLoginClick = this.handleLoginClick.bind(this);
        this.handleRegClick = this.handleRegClick.bind(this);
    }

    handleLoginClick() {
        this.setState({
            login: true,
            reg : false
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
                                                                <input type="text" name="username" id="username" tabIndex="1" className="form-control" placeholder="Username" value="" />
                                                            </div>
                                                            <div className="form-group">
                                                                <input type="password" name="password" id="password" tabIndex="2" className="form-control" placeholder="Password" />
                                                            </div>
                                                            <div className="form-group text-center">
                                                                <input type="checkbox" tabIndex="3" className="" name="remember" id="remember" />
                                                                <label for="remember"> Remember Me</label>
                                                            </div>
                                                            <div className="form-group">
                                                                <div className="row">
                                                                    <div className="col-sm-6 col-sm-offset-3">
                                                                        <input type="submit" name="login-submit" id="login-submit" tabIndex="4" className="form-control btn btn-login btn-primary" value="Log In" />
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