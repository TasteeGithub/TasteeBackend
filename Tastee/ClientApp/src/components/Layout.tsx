//Layout test
import * as React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

export default (props: { children?: React.ReactNode }) => {
    var loadScript = function (src: any) {
        var tag = document.createElement('script');
        tag.async = false;
        tag.src = src;
        document.getElementsByTagName('body')[0].appendChild(tag);
    }
    loadScript("/src/js/vendor/modernizr-2.8.3.min.js");
    //loadScript("https://code.jquery.com/jquery-3.3.1.min.js");
    loadScript("/src/js/vendor/jquery-3.3.1.min.js");
    loadScript("/plugins/popper.js/dist/umd/popper.min.js");
    loadScript("/plugins/bootstrap/dist/js/bootstrap.min.js");
    loadScript("/plugins/perfect-scrollbar/dist/perfect-scrollbar.min.js");
    loadScript("/plugins/screenfull/dist/screenfull.js");
    loadScript("/plugins/datatables.net/js/jquery.dataTables.min.js");
    loadScript("/plugins/datatables.net-bs4/js/dataTables.bootstrap4.min.js");
    loadScript("/plugins/datatables.net-responsive/js/dataTables.responsive.min.js");
    loadScript("/plugins/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js");
    loadScript("/plugins/jvectormap/jquery-jvectormap.min.js");
    loadScript("/plugins/jvectormap/tests/assets/jquery-jvectormap-world-mill-en.js");
    loadScript("/plugins/moment/moment.js");
    loadScript("/plugins/tempusdominus-bootstrap-4/build/js/tempusdominus-bootstrap-4.min.js");
    loadScript("/plugins/d3/dist/d3.min.js");
    loadScript("/plugins/c3/c3.min.js");
    loadScript("/plugins/datedropper/datedropper.min.js");
    loadScript("/plugins/jquery-toast-plugin/dist/jquery.toast.min.js");
    loadScript("/js/alerts.js");
    loadScript("/dist/js/theme.min.js");
    loadScript("/js/form-picker.js");
    return (

        <React.Fragment>
            <NavMenu />
            <Container>
                {props.children}
            </Container>
        </React.Fragment>
    );
}
