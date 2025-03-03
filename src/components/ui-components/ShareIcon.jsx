import React from "react";
import ShareIconStyle from "../../stylesheets/ShareIconStyle.module.css";

export const ShareIcon = () => {
  return (
    <div>
      <p className="color-0053A5 text-start text-uppercase">Share</p>
      <div
        className={`d-flex justify-content-between ${ShareIconStyle.icon_list}`}
      >
        <i className="fa-brands fa-linkedin-in fa-xl"></i>
        <i className="fa-brands fa-x-twitter fa-xl"></i>
        <i className="fa-brands fa-instagram fa-xl"></i>
        <i className="fa-brands fa-facebook-f fa-xl"></i>
        <i className="fa-solid fa-envelope fa-xl"></i>
        <i className="fa-solid fa-link-simple fa-xl"></i>
      </div>
    </div>
  );
};
