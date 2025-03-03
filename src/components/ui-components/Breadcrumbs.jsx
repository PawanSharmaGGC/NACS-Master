import React from "react";
import BreadcrumbsStyle from "../../stylesheets/BreadcrumbsStyle.module.css";

export const Breadcrumbs = ({ pages }) => {
  return (
    <div>
      <nav
        style={{
          "--bs-breadcrumb-divider":
            "url(\"data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='6' height='10'%3E%3Cpath d='M1.45508 8.96004L4.71508 5.70004C5.10008 5.31504 5.10008 4.68504 4.71508 4.30004L1.45508 1.04004' stroke='lightgrey' fill='none'/%3E%3C/svg%3E\")",
        }}
        aria-label="breadcrumb"
      >
        <ol className={`breadcrumb ${BreadcrumbsStyle.breadcrumb_list}`}>
          {pages.map((item, index) => (
            <li className={`breadcrumb-item `} key={index}>
              {item}
            </li>
          ))}
          {/* <li className="breadcrumb-item" aria-current="page"></li> */}
        </ol>
      </nav>
    </div>
  );
};
