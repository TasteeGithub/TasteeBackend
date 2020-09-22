import * as React from 'react';
import { connect } from 'react-redux';

const Dashboard = () => (
  <div>
        <h1>Welcome to Tastee.vn</h1>
    Website Tastee.vn đang bảo dưỡng, chúng tôi sẽ mở lại trong thời gian sớm nhất, quý khách vui lòng trở lại sau.
    </div>
);

export default connect()(Dashboard);
