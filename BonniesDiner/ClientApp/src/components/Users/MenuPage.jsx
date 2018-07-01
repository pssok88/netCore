﻿import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Counter';

class MenuPage extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [
                { "Id": "1", "ItemName": "Bbq Drumsticks"}
            ]
        }
           
    }

    getAllItems() {
        data: [
            { "Id": "1", "ItemName": "Bbq Drumsticks" }
        ]
    }

    render() {
        return (
            <div className="container">
                <h2 style={{ textAlign: "center" }}>Please confirm your order</h2>
                <div className="col-md-5 col-md-offset-3">
                    <div>
                        <h5>Name </h5>
                    </div> <br />
                    <div>
                        <h4>Order Details</h4>
                        <strong>Appetizers </strong> < br />
                        French Fries<br />
                        Chick-un Nuggets<br />
                        Cauliflower Buffalo Wings <br />  <br />
                        <strong>Entrees </strong> <br />
                        Chick-un Marinara Melt<br />
                        Spinach Artichoke Pesto Pizza<br />
                        Classic Veggie Burger<br />
                        Jackfruit Tacos<br />
                        Southwestern Quinoa Salad<br /> <br />
                        <strong>Dessert</strong> <br />
                        Carrot Cake<br />
                        Chocolate Cake<br />
                        Coconut Cake<br />
                        Ice Cream<br />
                        <br />
                    </div>
                    <br />
                    <div style={{ float: "right" }}>
                        <Button
                            className="btn btn-sm btn-danger"
                            onClick={this.getAllItems}
                            label="Edit"
                            disabled={false} />
                        <span style={{ paddingLeft: "10px" }}></span>
                        <Button
                            className="btn btn-sm btn-success"
                            onClick={this.getAllItems}
                            label="Submit"
                            disabled={false} />
                    </div>
                </div>
            </div>
        )
    }
};

//export default connect(
//    state => state.MenuPage,
//    dispatch => bindActionCreators(actionCreators, dispatch)
//)(MenuPage);
