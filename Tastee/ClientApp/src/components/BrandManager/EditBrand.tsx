import * as React from 'react';
import { RouteComponentProps, useParams, Redirect, useHistory } from 'react-router-dom';
import { useState, useEffect } from 'react';
import axios from 'axios';
import { stringify } from 'querystring';

export interface IValues {
    id: string,
    name: string,
    address: string,
    hotline: string,
    email: string,
    phone: string,
    headOffice: string,
    uri: string,
    logo: string,
    city: string,
    area: string,
    minPrice: number,
    maxPrice: number,
    status: string,
    createdDate: Date,
    updatedDate: Date,
    updateBy: null,
    metaDescription: string,
    seoTitle: string,
    seoDescription: string,
    seoImage: string,
    latitude: number,
    longitude: number,
    cuisines: string,
    categories: string,
    merchantId: string
}

const EditBrand: React.FunctionComponent<RouteComponentProps> = () => {
    var history = useHistory();
    const { id } = useParams();
    const [values, setValues] = useState({} as IValues);
    const [isSuccess, setSuccess] = useState(false);

    useEffect(() => {
        getData();
    }, []);

    const getData = async () => {
        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
            const result = await axios.get(`/api/brands/detail/${id}`);
            if (!result.data.data.id) history.push("/");
            await setValues(result.data.data);
            return;
        }
    }
    const handleSubmit = async (event: any) => {
        event.preventDefault();
        event.persist();

        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
            axios.post(`/api/brands/update/`, values)
                .then(result => {
                    if (result.data.successful) {
                        setSuccess(true);
                    }
                    else {
                        alert(stringify(result.data.error));
                        console.log(result);
                    }
                }).catch(e => {
                    alert(e.response);
                    console.log(e);
                });
        }
    };

    const handleChange = async (e: React.FormEvent<HTMLInputElement>) => {
        switch (e.currentTarget.id) {
            case "inputUri":
                setValues({ ...values, uri: e.currentTarget.value });
                break;
            case "inputBrandName":
                setValues({ ...values, name: e.currentTarget.value });
                break;
            case "inputAddress":
                setValues({ ...values, address: e.currentTarget.value });
                break;
            case "inputHotline":
                setValues({ ...values, hotline: e.currentTarget.value });
                break;
            case "inputEmail":
                setValues({ ...values, email: e.currentTarget.value });
                break;
            case "inputPhone":
                setValues({ ...values, phone: e.currentTarget.value });
                break;
            case "inputHeadOffice":
                setValues({ ...values, headOffice: e.currentTarget.value });
                break;
            case "inputMinPrice":
                setValues({ ...values, minPrice : parseInt(e.currentTarget.value,10) });
                break;
            case "inputMaxPrice":
                setValues({ ...values, maxPrice: parseInt(e.currentTarget.value, 10) });
                break;
            case "inputSeoTitle":
                setValues({ ...values, seoTitle: e.currentTarget.value });
                break;
            case "inputSeoDescription":
                setValues({ ...values, seoDescription: e.currentTarget.value });
                break;
            case "inputLatitube":
                setValues({ ...values, latitude: parseInt(e.currentTarget.value) });
                break;
            case "inputLongtitube":
                setValues({ ...values,  longitude : parseInt (e.currentTarget.value,10) });
                break;
        }
    }

    const handleSelectChange = async (e: React.FormEvent<HTMLInputElement>) => {
        switch (e.currentTarget.name) {
            case "inputCity":
                await setValues({ ...values, city: e.currentTarget.value });
                break;
            case "inputArea":
                await setValues({ ...values, area: e.currentTarget.value });
                break;
            case "inputOpenTime1":
                //brandInfo. = e.currentTarget.value;
                break;
            case "inputCloseTime1":
                //brandInfo = e.currentTarget.value;
                break;
            case "inputOpenTime2":
                //brandInfo = e.currentTarget.value;
                break;
            case "inputCloseTime2":
                //brandInfo = e.currentTarget.value;
                break;
            case "inputStatus":
                await setValues({ ...values, status: e.currentTarget.value });
                break;
            case "inputCuisines":
                await setValues({ ...values, cuisines: e.currentTarget.value });
                break;
            case "inputCategories":
                await setValues({ ...values, categories: e.currentTarget.value });
                break;
        }
    }

    if (isSuccess)
        return <Redirect to="/accounts" />
    else
        return (
            <div className="row">
                <div className="col-md-12">
                    <div className="card">
                        <form onSubmit={handleSubmit}>
                            <div className="card-body">
                                <h5 className="card-title"><strong>Info</strong></h5>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="forms-sample">
                                            <div className="form-group">
                                                <label htmlFor="inputUri">Uri</label>
                                                <input type="text" required className="form-control" id="inputUri" placeholder="Uri"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputBrandName">Brand's name</label>
                                                <input type="text" required className="form-control" id="inputBrandName" placeholder="Brand's name"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputAddress">Address</label>
                                                <input type="text" className="form-control" id="inputAddress" placeholder="Address"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputHotline">Hotline</label>
                                                <input type="text" className="form-control" id="inputHotline" placeholder="Hotline"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputEmail">Email</label>
                                                <input type="email" required className="form-control" id="inputEmail" placeholder="Email"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputPhone">Phone</label>
                                                <input type="text" required className="form-control" id="inputPhone" placeholder="Phone"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputHeadOffice">Head Office</label>
                                                <input type="text" className="form-control" id="inputHeadOffice" placeholder="Head Office"
                                                    onChange={handleChange} />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputRestaurantImage">Input Restaurant Image</label>
                                                <input type="text" className="form-control" id="inputRestaurantImage" />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="form-group">
                                            <label htmlFor="inputLogo">Logo</label>
                                            <input className="form-control" onChange={handleImageChange} type="file" id="inputLogo" placeholder="logo" />
                                            {$imagePrivew}
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="inputCity">City</label>
                                            <select className="form-control" id="inputCity" onChange={handleSelectChange}>
                                                <option value="HCM" selected={true}>Hồ Chí Minh</option>
                                                <option value="HN">Hà Nội</option>
                                                <option value="DN">Đà Nẵng</option>
                                            </select>
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="inputArea">Area</label>
                                            <select className="form-control" id="inputArea" onChange={handleSelectChange}>
                                                <option value="1" selected={true}>District 1</option>
                                                <option value="2">District 2</option>
                                                <option value="3">District 3</option>
                                            </select>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputOpenTime1">Open time 1</label>
                                                    <select className="form-control" id="inputOpenTime1" onChange={handleSelectChange}>
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option selected={true}>01:15</option>
                                                        <option>23:30</option>
                                                        <option>23:45</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCloseTime1">Close time 1</label>
                                                    <select className="form-control" id="inputCloseTime1" onChange={handleSelectChange}>
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option selected={true} >23:45</option>
                                                    </select>
                                                </div>

                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputOpenTime2">Open time 2</label>
                                                    <select className="form-control" id="inputOpenTime2" onChange={handleSelectChange}>
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option selected={true}>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option>23:45</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCloseTime2">Close time 2</label>
                                                    <select className="form-control" id="inputCloseTime2" onChange={handleSelectChange}>
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option selected={true} >23:45</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputMinPrice">Min Price (VNĐ)</label>
                                                    <input type="number" className="form-control" id="inputMinPrice" onChange={handleChange} />
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputMaxPrice">Max Price (VNĐ)</label>
                                                    <input type="number" className="form-control" id="inputMaxPrice" onChange={handleChange} />
                                                </div>
                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <label htmlFor="inputMetaDescription">Meta description</label>
                                            <textarea rows={5} className="form-control" id="inputMetaDescription" placeholder="Meta Description" onChange={handleTextAreaChange} />
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col-md-6">
                                        <h5 className="card-title"><strong>SEO</strong></h5>
                                        <div className="row">
                                            <div className="col-md-12">
                                                <div className="forms-sample">
                                                    <div className="form-group">
                                                        <label htmlFor="inputSeoTitle">SEO Title</label>
                                                        <input type="text" className="form-control" id="inputSeoTitle" placeholder="SEO Title" onChange={handleChange} />
                                                    </div>
                                                    <div className="form-group">
                                                        <label htmlFor="inputSeoDescription">SEO Description</label>
                                                        <input type="text" className="form-control" id="inputSeoDescription" onChange={handleChange} />
                                                    </div>
                                                    <div className="form-group">
                                                        <label htmlFor="inputSeoImage">SEO Image (GIF: Max 5MB,JPG,PNG,JPEG: Max 150KB)</label>
                                                        <input type="text" className="form-control" id="inputSeoImage" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <h5 className="card-title"><strong>Other Info</strong></h5>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputStatus">Status</label>
                                                    <select className="form-control" id="inputStatus" onChange={handleSelectChange}>
                                                        <option>Chờ kích hoạt</option>
                                                        <option>Đã kích hoạt</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputLatitube">Latitube</label>
                                                    <input type="number" className="form-control" id="inputLatitube" onChange={handleChange} />
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputLongtitube">Longtitube</label>
                                                    <input type="number" className="form-control" id="inputLongtitube" onChange={handleChange} />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCuisines">Cuisines</label>
                                                    <select className="form-control" id="inputCuisines" onChange={handleSelectChange}>
                                                        <option>Món Bắc</option>
                                                        <option>Món Miền Trung</option>
                                                        <option>Món Miền Nam</option>
                                                        <option>Món Tây Nguyên</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCategories">Categories</label>
                                                    <select className="form-control" id="inputCategories" onChange={handleSelectChange}>
                                                        <option>Buffet</option>
                                                        <option>Birthday</option>
                                                        <option>Sushi</option>
                                                        <option>Dimsum</option>
                                                        <option>Chinese</option>
                                                        <option>....</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="card-footer text-right">
                                <button type="submit" className="btn btn-primary mr-2"><i className="ik ik-save" />Save</button>
                                <button className="btn btn-light">Cancel</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        );
}
export default EditBrand;