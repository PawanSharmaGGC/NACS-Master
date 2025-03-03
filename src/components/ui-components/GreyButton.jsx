import React from "react";
import { Button } from "react-bootstrap";

export const GreyButton = ({
  name,
  fontSize,
  siconType,
  siconName,
  siconColor = "color-0053A5",
  siconSize = "",
  ficonType,
  ficonName,
  ficonColor = "color-0053A5",
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
        className={`bg-F5F5F5 p-3 border border-0 brdr-rad-30 text-decoration-none ${btnClass}`}
        onClick={buttonClick}
      >
        <span>
          <i
            className={`align-middle ${ficonColor} ${ficonType} ${ficonName} ${ficonSize}`}
          ></i>
        </span>
        <span className={`color-0053A5 pe-2 ps-2 fw-medium ${fontSize}`}>
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
