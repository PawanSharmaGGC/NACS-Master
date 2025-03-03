import React from "react";
import { Button } from "react-bootstrap";

export const SideRoundButton = ({
  name,
  fontSize,
  siconType = "fa-regular",
  siconName = "fa-arrow-right-long",
  siconColor = "color-FFFFFF",
  siconSize = "",
  ficonType,
  ficonName,
  ficonColor = "color-FFFFFF",
  ficonSize = "",
  className = "",
  handleClick = () => {},
  btnClass = "",
}) => {
  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  };

  const buttonClick = () => {
    scrollToTop();
    handleClick();
  };
  return (
    <div className={className}>
      <a
        href="#"
        className={`bg-0053A5 p-3 border border-0 brdr-rad-30 text-decoration-none ${btnClass}`}
        onClick={buttonClick}
      >
        <span>
          <i
            className={`align-middle ${ficonColor} ${ficonType} ${ficonName} ${ficonSize}`}
          ></i>
        </span>
        <span className={`color-FFFFFF pe-2 ps-2 fw-medium ${fontSize}`}>
          {name + "  "}
        </span>
        <span>
          <i
            className={`align-middle ${siconColor} ${siconType} ${siconName} ${siconSize}`}
          ></i>
        </span>
      </a>
    </div>
  );
};
