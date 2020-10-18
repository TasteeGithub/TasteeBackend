
//Lay out cho backend
import React from 'react';
import LoginStatus from './Authentication/LoginStatus';
import { Link } from 'react-router-dom';

const Main: React.FunctionComponent = (props: { children?: React.ReactNode }) => {
    var loadScript = function (src: any) {
        var tag = document.createElement('script');
        tag.async = false;
        tag.src = src;
        document.getElementsByTagName('body')[0].appendChild(tag);
    }
    loadScript("/src/js/vendor/modernizr-2.8.3.min.js");
    loadScript("https://code.jquery.com/jquery-3.3.1.min.js");
    loadScript("/src/js/vendor/jquery-3.3.1.min.js");
    //loadScript("/plugins/popper.js/dist/umd/popper.min.js");
    //loadScript("/plugins/bootstrap/dist/js/bootstrap.min.js");
    //loadScript("/plugins/perfect-scrollbar/dist/perfect-scrollbar.min.js");
    //loadScript("/plugins/screenfull/dist/screenfull.js");
    //loadScript("/plugins/datatables.net/js/jquery.dataTables.min.js");
    //loadScript("/plugins/datatables.net-bs4/js/dataTables.bootstrap4.min.js");
    //loadScript("/plugins/datatables.net-responsive/js/dataTables.responsive.min.js");
    //loadScript("/plugins/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js");
    //loadScript("/plugins/jvectormap/jquery-jvectormap.min.js");
    //loadScript("/plugins/jvectormap/tests/assets/jquery-jvectormap-world-mill-en.js");
    //loadScript("/plugins/moment/moment.js");
    //loadScript("/plugins/tempusdominus-bootstrap-4/build/js/tempusdominus-bootstrap-4.min.js");
    //loadScript("/plugins/d3/dist/d3.min.js");
    //loadScript("/plugins/c3/c3.min.js");
    //loadScript("/plugins/datedropper/datedropper.min.js");
    //loadScript("/plugins/jquery-toast-plugin/dist/jquery.toast.min.js");
    //loadScript("/plugins/jquery.repeater/jquery.repeater.min.js");
    //loadScript("/js/alerts.js");k
    //loadScript("/dist/js/theme.min.js");
    //loadScript("/js/form-picker.js");
    //loadScript("/js/form-advanced.js");
   
    return (
        <>

            <header className="header-top" header-theme="light">
                <div className="container-fluid">
                    <div className="d-flex justify-content-between">
                        <div className="top-menu d-flex align-items-center">
                            <button type="button" className="btn-icon mobile-nav-toggle d-lg-none"><span></span></button>
                            <div className="header-search">
                                <div className="input-group">
                                    <span className="input-group-addon search-close"><i className="ik ik-x"></i></span>
                                    <input type="text" className="form-control" />
                                    <span className="input-group-addon search-btn"><i className="ik ik-search"></i></span>
                                </div>
                            </div>
                            <button type="button" id="navbar-fullscreen" className="nav-link"><i className="ik ik-maximize"></i></button>
                        </div>
                        
                        <div className="top-menu d-flex align-items-center">
                            {/*  TODO : Phần này tích hợp các thông báo cho operator
                            <div className="dropdown">
                                <a className="nav-link dropdown-toggle" href="#" id="notiDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i className="ik ik-bell"></i><span className="badge bg-danger">3</span></a>
                                <div className="dropdown-menu dropdown-menu-right notification-dropdown" aria-labelledby="notiDropdown">
                                    <h4 className="header">Notifications</h4>
                                    <div className="notifications-wrap">
                                        <a href="#" className="media">
                                            <span className="d-flex">
                                                <i className="ik ik-check"></i>
                                            </span>
                                            <span className="media-body">
                                                <span className="heading-font-family media-heading">Invitation accepted</span>
                                                <span className="media-content">Your have been Invited ...</span>
                                            </span>
                                        </a>
                                        <a href="#" className="media">
                                            <span className="d-flex">
                                                <img src="img/users/1.jpg" className="rounded-circle" alt="" />
                                            </span>
                                            <span className="media-body">
                                                <span className="heading-font-family media-heading">Steve Smith</span>
                                                <span className="media-content">I slowly updated projects</span>
                                            </span>
                                        </a>
                                        <a href="#" className="media">
                                            <span className="d-flex">
                                                <i className="ik ik-calendar"></i>
                                            </span>
                                            <span className="media-body">
                                                <span className="heading-font-family media-heading">To Do</span>
                                                <span className="media-content">Meeting with Nathan on Friday 8 AM ...</span>
                                            </span>
                                        </a>
                                    </div>
                                    <div className="footer"><a href="#" onClick={ev => ev.preventDefault()} >See all activity</a></div>
                                </div>
                            </div>
                            <button type="button" className="nav-link ml-10 right-sidebar-toggle"><i className="ik ik-message-square"></i><span className="badge bg-success">3</span></button>
                            <div className="dropdown">
                                <a className="nav-link dropdown-toggle" href="#" id="menuDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i className="ik ik-plus"></i></a>
                                <div className="dropdown-menu dropdown-menu-right menu-grid" aria-labelledby="menuDropdown">
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Dashboard"><i className="ik ik-bar-chart-2"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Message"><i className="ik ik-mail"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Accounts"><i className="ik ik-users"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Sales"><i className="ik ik-shopping-cart"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Purchase"><i className="ik ik-briefcase"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Pages"><i className="ik ik-clipboard"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Chats"><i className="ik ik-message-square"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Contacts"><i className="ik ik-map-pin"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Blocks"><i className="ik ik-inbox"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Events"><i className="ik ik-calendar"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="Notifications"><i className="ik ik-bell"></i></a>
                                    <a className="dropdown-item" href="#" data-toggle="tooltip" data-placement="top" title="More"><i className="ik ik-more-horizontal"></i></a>
                                </div>
                            </div>
                            <button type="button" className="nav-link ml-10" id="apps_modal_btn" data-toggle="modal" data-target="#appsModal"><i className="ik ik-grid"></i></button>
                            */}
                            <LoginStatus />

                        </div>
                    </div>
                </div>
            </header>
            <div className="page-wrap">
                <div className="app-sidebar colored">
                    <div className="sidebar-header">
                        <a className="header-brand" href="/dashboard">
                            <div className="logo-img">
                                {/*<img src="src/img/brand-white.svg" className="header-brand-img" alt="lavalite" />
                                */}
                            </div>
                            <span className="text">TASTEE</span>
                        </a>
                        <button type="button" className="nav-toggle"><i data-toggle="expanded" className="ik ik-toggle-right toggle-icon"></i></button>
                        <button id="sidebarClose" className="nav-close"><i className="ik ik-x"></i></button>
                    </div>

                    <div className="sidebar-content">
                        <div className="nav-container">
                            <nav id="main-menu-navigation" className="navigation-main">
                                <div className="nav-item">
                                    <Link to="/dashboard/"><i className="ik ik-bar-chart-2"></i><span>Dashboard</span></Link>
                                </div>
                                <div className="nav-item">
                                    <Link to="/accounts/"><i className="ik ik-users"></i><span>Operators</span></Link>
                                </div>
                                <div className="nav-item">
                                    <Link to="/create-account/"><i className="ik ik-user-plus"></i><span>New Operator</span></Link>
                                </div>
                                <div className="nav-item">
                                    <Link to="/users/"><i className="ik ik-users"></i><span>Users</span></Link>
                                </div>
                                <div className="nav-item">
                                    <Link to="/brands/"><i className="ik ik-server"></i><span>Brands</span></Link>
                                </div>
                            </nav>
                        </div>
                    </div>
                </div>
                <div className="main-content">
                    <div className="container-fluid">
                        {props.children}
                    </div>
                </div>

                <aside className="right-sidebar">
                    <div className="sidebar-chat" data-plugin="chat-sidebar">
                        <div className="sidebar-chat-info">
                            <h6>Chat List</h6>
                            <form className="mr-t-10">
                                <div className="form-group">
                                    <input type="text" className="form-control" placeholder="Search for friends ..." />
                                    <i className="ik ik-search"></i>
                                </div>
                            </form>
                        </div>
                        <div className="chat-list">
                            <div className="list-group row">
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item" data-chat-user="Gene Newman">
                                    <figure className="user--online">
                                        <img src="img/users/1.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Gene Newman</span>  <span className="username">@gene_newman</span> </span>
                                </a>
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item" data-chat-user="Billy Black">
                                    <figure className="user--online">
                                        <img src="img/users/2.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Billy Black</span>  <span className="username">@billyblack</span> </span>
                                </a>
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item" data-chat-user="Herbert Diaz">
                                    <figure className="user--online">
                                        <img src="img/users/3.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Herbert Diaz</span>  <span className="username">@herbert</span> </span>
                                </a>
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item" data-chat-user="Sylvia Harvey">
                                    <figure className="user--busy">
                                        <img src="img/users/4.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Sylvia Harvey</span>  <span className="username">@sylvia</span> </span>
                                </a>
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item active" data-chat-user="Marsha Hoffman">
                                    <figure className="user--busy">
                                        <img src="img/users/5.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Marsha Hoffman</span>  <span className="username">@m_hoffman</span> </span>
                                </a>
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item" data-chat-user="Mason Grant">
                                    <figure className="user--offline">
                                        <img src="img/users/1.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Mason Grant</span>  <span className="username">@masongrant</span> </span>
                                </a>
                                <a href="#" onClick={ev => ev.preventDefault()} className="list-group-item" data-chat-user="Shelly Sullivan">
                                    <figure className="user--offline">
                                        <img src="img/users/2.jpg" className="rounded-circle" alt="" />
                                    </figure><span><span className="name">Shelly Sullivan</span>  <span className="username">@shelly</span></span>
                                </a>
                            </div>
                        </div>
                    </div>
                </aside>

                <div className="chat-panel" hidden>
                    <div className="card">
                        <div className="card-header d-flex justify-content-between">
                            <a href="#" onClick={ev => ev.preventDefault()}><i className="ik ik-message-square text-success"></i></a>
                            <span className="user-name">John Doe</span>
                            <button type="button" className="close" aria-label="Close"><span aria-hidden="true">×</span></button>
                        </div>
                        <div className="card-body">
                            <div className="widget-chat-activity flex-1">
                                <div className="messages">
                                    <div className="message media reply">
                                        <figure className="user--online">
                                            <a href="#">
                                                <img src="img/users/3.jpg" className="rounded-circle" alt="" />
                                            </a>
                                        </figure>
                                        <div className="message-body media-body">
                                            <p>Epic Cheeseburgers come in all kind of styles.</p>
                                        </div>
                                    </div>
                                    <div className="message media">
                                        <figure className="user--online">
                                            <a href="#">
                                                <img src="img/users/1.jpg" className="rounded-circle" alt="" />
                                            </a>
                                        </figure>
                                        <div className="message-body media-body">
                                            <p>Cheeseburgers make your knees weak.</p>
                                        </div>
                                    </div>
                                    <div className="message media reply">
                                        <figure className="user--offline">
                                            <a href="#">
                                                <img src="img/users/5.jpg" className="rounded-circle" alt="" />
                                            </a>
                                        </figure>
                                        <div className="message-body media-body">
                                            <p>Cheeseburgers will never let you down.</p>
                                            <p>They'll also never run around or desert you.</p>
                                        </div>
                                    </div>
                                    <div className="message media">
                                        <figure className="user--online">
                                            <a href="#">
                                                <img src="img/users/1.jpg" className="rounded-circle" alt="" />
                                            </a>
                                        </figure>
                                        <div className="message-body media-body">
                                            <p>A great cheeseburger is a gastronomical event.</p>
                                        </div>
                                    </div>
                                    <div className="message media reply">
                                        <figure className="user--busy">
                                            <a href="#">
                                                <img src="img/users/5.jpg" className="rounded-circle" alt="" />
                                            </a>
                                        </figure>
                                        <div className="message-body media-body">
                                            <p>There's a cheesy incarnation waiting for you no matter what you palete preferences are.</p>
                                        </div>
                                    </div>
                                    <div className="message media">
                                        <figure className="user--online">
                                            <a href="#">
                                                <img src="img/users/1.jpg" className="rounded-circle" alt="" />
                                            </a>
                                        </figure>
                                        <div className="message-body media-body">
                                            <p>If you are a vegan, we are sorry for you loss.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <form className="card-footer" method="post">
                            <div className="d-flex justify-content-end">
                                <textarea className="border-0 flex-1" rows={1} placeholder="Type your message here"></textarea>
                                <button className="btn btn-icon" type="submit"><i className="ik ik-arrow-right text-success"></i></button>
                            </div>
                        </form>
                    </div>
                </div>

                <footer className="footer">
                    <div className="w-100 clearfix">
                        <span className="text-center text-sm-left d-md-inline-block">Copyright © 2020 Tastee All Rights Reserved.</span>
                    </div>
                </footer>

            </div>

        </>
    );
}

export default Main;