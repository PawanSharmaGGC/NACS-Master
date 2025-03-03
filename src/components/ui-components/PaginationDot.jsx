import React from "react";
import PaginationDotStyle from "../../stylesheets/PaginationDot.module.css";

export const PaginationDot = ({
  currElement,
  data,
  next,
  previous,
  position = "justify-content-lg-start",
}) => {
  return (
    <div>
      <div className={`d-flex ${position} justify-content-evenly mt-4 ps-lg-3`}>
        <p
          className={`pe-5 pointer ${
            currElement === 0 ? "color-868F98" : "color-0053A5"
          }`}
          onClick={currElement === 0 ? null : previous}
        >
          <i className="fa-regular fa-angle-left fa-xl"></i>
          <span className="ps-1">Prev</span>
        </p>
        <p className="pe-5">
          <div className={`${PaginationDotStyle.carousel_dots}`}>
            {data.map((item, index) => (
              <span
                className={`${
                  currElement === index ? PaginationDotStyle.active_dot : ""
                } ${PaginationDotStyle.dot}`}
              ></span>
            ))}
          </div>
        </p>
        <p
          className={`pointer ${
            currElement === data.length - 1 ? "color-868F98" : "color-0053A5"
          }`}
          onClick={currElement === data.length - 1 ? null : next}
        >
          <span className="pe-1">Next</span>
          <i className="fa-regular fa-angle-right fa-xl "></i>
        </p>
      </div>
    </div>
  );
};
