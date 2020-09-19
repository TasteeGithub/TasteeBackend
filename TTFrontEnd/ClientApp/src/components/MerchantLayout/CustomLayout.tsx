﻿import * as React from 'react';
import Demobar from '../../demobar';
import * as variables from '../../variables'
import FormBuilders from '../../FormBuilders';
//const $ = require('jquery');
import './app.css';

const CustomLayout: React.FunctionComponent = () => {

    return (

        <div>
            <div>
                <Demobar variables={variables} />
                <FormBuilders.ReactFormBuilder variables={variables}
                    data={[{
                        "id": "A8C5D4D4-12D9-4878-8BFA-E0C1D2FD19D7", "element": "Label", "text": "Label"
                        , "static": true, "required": false, "bold": false, "italic": false
                        , "content": "Placeholder Text...44444", "canHavePageBreakBefore": true, "canHaveAlternateForm": true
                        , "canHaveDisplayHorizontal": true, "canHaveOptionCorrect": true, "canHaveOptionValue": true
                        , "canPopulateFromApi": true
                    },
                        {
                            "id": "E7D986D2-6DBF-4753-8567-4C0C9B11D9AE", "element": "Dropdown", "text": "Dropdown"
                            , "required": false, "canHaveAnswer": true, "canHavePageBreakBefore": true
                            , "canHaveAlternateForm": true, "canHaveDisplayHorizontal": true, "canHaveOptionCorrect": true
                            , "canHaveOptionValue": true, "canPopulateFromApi": true
                            , "field_name": "dropdown_19148A71-6C34-4620-9BDA-FB6AC359576C", "label": "Placeholder Label"
                            , "options": [{
                                "value": "place_holder_option_1", "text": "Place holder option 1"
                                , "key": "dropdown_option_97E816F5-711D-48FE-9ADE-E727340F1E2F"
                            }, {
                                "value": "place_holder_option_2", "text": "Place holder option 2"
                                    , "key": "dropdown_option_E72DD19B-AF4C-4564-A46D-EAF8E338C6A2"
                                }, {
                                    "value": "place_holder_option_3", "text": "Place holder option 3"
                                    , "key": "dropdown_option_17CCD7B6-4887-4CC2-9F87-C9CDC0E659E0"
                                }]
                        }
                    ]} />
            </div>
            
        </div>
        );
}
export default CustomLayout;