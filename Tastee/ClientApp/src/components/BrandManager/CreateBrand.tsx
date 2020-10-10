import * as React from 'react';
import axios from 'axios';
import { Redirect } from 'react-router';
const $ = require('jquery');

interface BrandInfo {
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
    maxPrice: number ,
    status: string,
    metaDescription: string,
    seoTitle: string,
    seoDescription: string,
    seoImage: string,
    latitude: number,
    longitude: number,
    cuisines: string,
    categories: string
};

interface IState {
    imgagePreviewUrl: string | undefined,
    logoFile: any,
    isFinished: boolean,
}

class CreateBrand extends React.PureComponent<{}, IState> {
    constructor(props: any) {
        super(props);
        this.state = { imgagePreviewUrl: "", logoFile: null, isFinished: false};
    }
    inputBirth: any;
    $inputBirth: any;
    componentDidMount = () => {
        //this.$inputBirth = this.inputBirth;

        //let dateDropper = $.fn.dateDropper; //accessing jquery function

        //$("#inputBithday").dateDropper({
        //    dropWidth: 200,
        //    dropPrimaryColor: "#1abc9c",
        //    dropBorder: "1px solid #1abc9c"
        //});

        //this.$inputBirth = $(this.inputBirth);
        //alert(this.$inputBirth.id);
        //this.$inputBirth.dateDropper({
        //    dropWidth: 200,
        //    dropPrimaryColor: "#1abc9c",
        //    dropBorder: "1px solid #1abc9c"
        //});
    }

    brandInfo: BrandInfo = {
        name: "",
        address: "",
        hotline: "",
        email: "",
        phone: "",
        headOffice: "",
        uri: "",
        logo: "",
        city: "",
        area: "",
        minPrice: 0,
        maxPrice: 0,
        status: "",
        metaDescription: "",
        seoTitle: "",
        seoDescription: "",
        seoImage: "",
        latitude: 0,
        longitude: 0,
        cuisines: "",
        categories: ""
    };
    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
        this.CreateBrand();
    }

    CreateBrand = async () => {
        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
        }
        else
            return;

        //if (this.state.avatarFile?.name != null) {
        //    let formData = new FormData();
        //    formData.append("myFile", this.state.avatarFile, this.state.avatarFile.name);
        //    let rs = await axios.post("/api/operators/uploadfile", formData);

        //    if (rs.status == 200) {
        //        this.accountInfo.avatar = rs.data.newFileName;
        //    }
        //    else {
        //        alert(rs.status);
        //        return;
        //    }
        //}

        let brandModel = {
            name: this.brandInfo.name,
            address: this.brandInfo.address,
            hotline: this.brandInfo.hotline,
            email: this.brandInfo.email,
            phone: this.brandInfo.phone,
            headOffice: this.brandInfo.headOffice,
            uri: this.brandInfo.uri,
            logo: this.state.logoFile,
            city: this.brandInfo.city,
            area: this.brandInfo.area,
            minPrice: this.brandInfo.minPrice,
            maxPrice: this.brandInfo.maxPrice,
            status: this.brandInfo.status,
            metaDescription: this.brandInfo.metaDescription,
            seoTitle: this.brandInfo.seoTitle,
            seoDescription: this.brandInfo.seoDescription,
            seoImage: this.brandInfo.seoImage,
            latitude: this.brandInfo.latitude,
            longitude: this.brandInfo.longitude,
            cuisines: this.brandInfo.cuisines,
            categories: this.brandInfo.categories
        }
        alert(brandModel.minPrice);
        axios.post("/api/brands", brandModel)
            .then((rs) => {
                if (rs.data.successful)
                    this.setState({
                        ...this.state,
                        isFinished: true
                    });
                else
                    alert(rs.data.error[0]);
            })
            .catch((e) => {
                if (e.response) {
                    var error = JSON.parse(e.response.request.response).errors;
                    console.log(error);
                    alert(error);
                }
                else if (e.request) {
                    alert(e + ",rs");
                }
            });
    }

    handleSelectChange = (e: React.FormEvent<HTMLSelectElement>) => {
        switch (e.currentTarget.name) {
            case "inputCity":
                this.brandInfo.city = e.currentTarget.value;
                break;
            case "inputArea":
                this.brandInfo.area = e.currentTarget.value;
                break;
            case "inputOpenTime1":
                //this.brandInfo. = e.currentTarget.value;
                break;
            case "inputCloseTime1":
                //this.brandInfo = e.currentTarget.value;
                break;
            case "inputOpenTime2":
                //this.brandInfo = e.currentTarget.value;
                break;
            case "inputCloseTime2":
                //this.brandInfo = e.currentTarget.value;
                break;
            case "inputStatus":
                this.brandInfo.status = e.currentTarget.value;
                break;
            case "inputCuisines":
                this.brandInfo.cuisines = e.currentTarget.value;
                break;
            case "inputCategories":
                this.brandInfo.categories = e.currentTarget.value;
                break;
        }
    }
    handleTextAreaChange = (e: React.FormEvent<HTMLTextAreaElement>) => {
        this.brandInfo.metaDescription = e.currentTarget.value;
    }
    handleChange = (e: React.FormEvent<HTMLInputElement>) => {
        switch (e.currentTarget.id) {
            case "inputUri":
                this.brandInfo.uri = e.currentTarget.value;
                break;
            case "inputBrandName":
                this.brandInfo.name = e.currentTarget.value;
                break;
            case "inputAddress":
                this.brandInfo.address = e.currentTarget.value;
                break;
            case "inputHotline":
                this.brandInfo.hotline = e.currentTarget.value;
                break;
            case "inputEmail":
                this.brandInfo.email = e.currentTarget.value;
                break;
            case "inputPhone":
                this.brandInfo.phone = e.currentTarget.value;
                break;
            case "inputHeadOffice":
                this.brandInfo.headOffice = e.currentTarget.value;
                break;
            case "inputMinPrice":
                this.brandInfo.minPrice = parseInt(e.currentTarget.value,10)  ;
                break;
            case "inputMaxPrice":
                this.brandInfo.maxPrice = parseInt(e.currentTarget.value);
                break;
            case "inputSeoTitle":
                this.brandInfo.seoTitle = e.currentTarget.value;
                break;
            case "inputSeoDescription":
                this.brandInfo.seoDescription = e.currentTarget.value;
                break;
            case "inputLatitube":
                this.brandInfo.latitude = parseFloat(e.currentTarget.value);
                break;
            case "inputLongtitube":
                this.brandInfo.longitude = parseFloat(e.currentTarget.value);
                break;
        }
    }

    //handleGetRole = (roleId: string) => {
    //    this.accountInfo.roleId = roleId;
    //}

    handleImageChange = (e: React.FormEvent<HTMLInputElement>) => {
        e.preventDefault();

        let reader = new FileReader();

        let file = e.currentTarget.files == null ? null : e.currentTarget.files[0];

        if (file != null) {
            reader.onloadend = () => {
                this.setState({
                    imgagePreviewUrl: reader.result?.toString(),
                    logoFile: file?.name ,
                    isFinished: this.state.isFinished,
                });
            }
            reader.readAsDataURL(file);
        }
    }

    render() {
        if (this.state.isFinished) return <Redirect to="/brands" />;
        let imagePreviewUrl = this.state.imgagePreviewUrl;
        let $imagePrivew = null;
        if (imagePreviewUrl) {
            $imagePrivew = (<div style={{ paddingTop: "20px" }}><img style={{ maxWidth: "400px" }} src={imagePreviewUrl} /></div>);
        }

        return (
            <div className="row">
                <div className="col-md-12">
                    <div className="card">
                        <form onSubmit={this.handleSubmit}>
                            <div className="card-body">
                                <h5 className="card-title"><strong>Info</strong></h5>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="forms-sample">
                                            <div className="form-group">
                                                <label htmlFor="inputUri">Uri</label>
                                                <input type="text" required className="form-control" id="inputUri" placeholder="Uri"
                                                    onChange={this.handleChange}/>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputBrandName">Brand's name</label>
                                                <input type="text" required className="form-control" id="inputBrandName" placeholder="Brand's name"
                                                    onChange={this.handleChange}/>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputAddress">Address</label>
                                                <input type="text" className="form-control" id="inputAddress" placeholder="Address"
                                                    onChange={this.handleChange}/>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputHotline">Hotline</label>
                                                <input type="text" className="form-control" id="inputHotline" placeholder="Hotline"
                                                    onChange={this.handleChange}/>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputEmail">Email</label>
                                                <input type="email" required className="form-control" id="inputEmail" placeholder="Email"
                                                    onChange={this.handleChange}/>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputPhone">Phone</label>
                                                <input type="text" required className="form-control" id="inputPhone" placeholder="Phone"
                                                    onChange={this.handleChange}/>
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputHeadOffice">Head Office</label>
                                                <input type="text" className="form-control" id="inputHeadOffice" placeholder="Head Office"
                                                    onChange={this.handleChange}/>
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
                                            <input className="form-control" onChange={this.handleImageChange} type="file" id="inputLogo" placeholder="logo" />
                                            {$imagePrivew}
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="inputCity">City</label>
                                            <select className="form-control" id="inputCity" onChange={this.handleSelectChange}>
                                                <option value="HCM" selected={true}>Hồ Chí Minh</option>
                                                <option value="HN">Hà Nội</option>
                                                <option value="DN">Đà Nẵng</option>
                                            </select>
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="inputArea">Area</label>
                                            <select className="form-control" id="inputArea" onChange={this.handleSelectChange}>
                                                <option value="1" selected={true}>District 1</option>
                                                <option value="2">District 2</option>
                                                <option value="3">District 3</option>
                                            </select>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputOpenTime1">Open time 1</label>
                                                    <select className="form-control" id="inputOpenTime1" onChange={this.handleSelectChange}>
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
                                                    <select className="form-control" id="inputCloseTime1" onChange={this.handleSelectChange}>
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
                                                    <select className="form-control" id="inputOpenTime2" onChange={this.handleSelectChange}>
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
                                                    <select className="form-control" id="inputCloseTime2" onChange={this.handleSelectChange}>
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
                                                    <input type="number" className="form-control" id="inputMinPrice" onChange={this.handleChange}/>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputMaxPrice">Max Price (VNĐ)</label>
                                                    <input type="number" className="form-control" id="inputMaxPrice" onChange={this.handleChange}/>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <label htmlFor="inputMetaDescription">Meta description</label>
                                            <textarea rows={5} className="form-control" id="inputMetaDescription" placeholder="Meta Description" onChange={this.handleTextAreaChange}/>
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
                                                        <input type="text" className="form-control" id="inputSeoTitle" placeholder="SEO Title" onChange={this.handleChange}/>
                                                    </div>
                                                    <div className="form-group">
                                                        <label htmlFor="inputSeoDescription">SEO Description</label>
                                                        <input type="text" className="form-control" id="inputSeoDescription" onChange={this.handleChange}/>
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
                                                    <select className="form-control" id="inputStatus" onChange={this.handleSelectChange}>
                                                        <option value = "0">Chờ kích hoạt</option>
                                                        <option value="1">Đã kích hoạt</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputLatitube">Latitube</label>
                                                    <input type="number" className="form-control" id="inputLatitube" onChange={this.handleChange}/>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputLongtitube">Longtitube</label>
                                                    <input type="number" className="form-control" id="inputLongtitube" onChange={this.handleChange}/>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCuisines">Cuisines</label>
                                                    <select className="form-control" id="inputCuisines" onChange={this.handleSelectChange}>
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
                                                    <select className="form-control" id="inputCategories" onChange={this.handleSelectChange}>
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
                              </div>
                        </form>
                    </div>
                </div>
            </div>
        );
    }
}

export default CreateBrand;