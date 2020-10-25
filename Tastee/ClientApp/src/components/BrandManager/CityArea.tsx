import React, { Component } from 'react';

interface IArea {
    value: string,
    text: string
}

interface CArea {
    areas:[string,IArea[]]
}

interface IAreaState {
    City: string,
    Areas : IArea[]
}

export default class CityArea extends Component<{}, IAreaState>{

    constructor(props: any) {
        super(props);
        this.state = {City:"",Areas:[]}
        this.initCity();
    };

    getAreaByCity = (city: string): IAreaState => {
        switch (city) {
            case 'HCM':
                return {
                    City: "HCM",
                    Areas: [
                        { value: "1", text: "Quận 1" },
                        { value: "2", text: "Quận 2" },
                        { value: "3", text: "Quận 3" },
                        { value: "4", text: "Quận 4" },
                        { value: "5", text: "Quận 5" },
                        { value: "6", text: "Quận 6" }
                    ]
                };
            case 'HN':
                return {
                    City: "HN",
                    Areas: [
                        { value: "5646", text: "Ba Đình" },
                        { value: "3535", text: "Hoàn Kiếm" },
                        { value: "535", text: "Hai Bà Trưng" },
                        { value: "55", text: "Hoàng Mai" },
                        { value: "35", text: "Thanh Xuân" },
                    ]
                };
            default:
                return {
                    City: "",
                    Areas:[]
                };
        }
    }


    initCity = () => {
        this.setState(this.getAreaByCity("HN"));
    }

    handleCityChange = (e: React.FormEvent<HTMLSelectElement>) => {
        e.preventDefault();
        this.setState(this.getAreaByCity(e.currentTarget.value));
    }
    render() {
        return (
            <>
                <div className="form-group">
                    <label htmlFor="inputCity">City</label>
                    <select className="form-control" id="inputCity" onChange={ this.handleCityChange }>
                        <option value="HCM" selected={true}>Hồ Chí Minh</option>
                        <option value="HN">Hà Nội</option>
                        <option value="DN">Đà Nẵng</option>
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="inputArea">Area</label>
                    <select className="form-control" id="inputArea">
                        {
                            this.state.Areas.map(
                                (area) => {
                                    return <option value={area.value}>{area.text}</option>;
                            })
                        //    <option value="1" selected={true}>District 1</option>
                        //<option value="2">District 2</option>
                        //<option value="3">District 3</option>
                        //<option value="3">District 4</option>
                        }
                    </select>
                </div>
                </>
            
            );
    }
}