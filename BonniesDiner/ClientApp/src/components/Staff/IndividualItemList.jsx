﻿import * as React from "react";

export const IndividualItemList = (props) => {
    return (
        <React.Fragment>
            <div className="row">
                {props.headerColumns != null ? (
                    props.headerColumns.map((itm, i) => {
                        return <div key={i} className={itm.columnStyle}><strong>{itm.columnName}</strong></div>
                    })
                )
                    : ("")
                }
            </div>
            {props.dataItems.map(buildRow(props))}
        </React.Fragment>
    )
}

const buildRow = (props) => (itm, ndx) => {
    return (<div className="container col-md-12" key={ndx}>
        <br />
        <div className="row">
            <div className="col-md-1"><strong>{itm.orderId}</strong></div>
            <div className="col-md-1"><strong>{itm.itemId}</strong></div>
            <div className="col-md-1"><strong>{itm.itemQty}</strong></div>
            <div className="col-md-3">{itm.statusNew}</div>
            <div className="col-md-3">{itm.statusFulfilled}</div>
            <div className="col-md-3">{itm.statusCancelled}</div>
        </div>
        <hr />
    </div>);
};