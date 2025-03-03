import React from "react";

export const TextLinkButton = ({ name, handleClick = () => {} }) => {
  return (
    <a
      href="#"
      className="fs-5 color-396a99 pointer text-decoration-none"
      onClick={handleClick}
    >
      <span className="d-inline-block pe-1 fw-medium">{name}</span>{" "}
      <span className="align-middle">
        <i className="fa-regular fa-arrow-right-long color-396a99"></i>
      </span>
    </a>
  );
};
