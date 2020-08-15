import * as React from 'react';
const $ = require('jquery');

const CustomLayout: React.FunctionComponent = () => {

    const componentDidMount = () => {
        alert($("#mylable").InnerText);
    }

    return (
        <div>
            <div>
                <label id="mylable">kkkkkkkkk</label>
            </div>

        </div>
        );
}
export default CustomLayout;