import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import { Button } from "react-bootstrap";

export const InvertedButton = ({ name }) => {
  return (
    <div>
      <Button className="bg-FFFFFF border border-0 brdr-rad-18">
        <span className="color-0053A5">{name + "  "}</span>
        <i className="color-0053A5 fa-regular fa-arrow-right"></i>
        {/* <svg
          width="14"
          height="10"
          viewBox="0 0 14 10"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <g clip-path="url(#clip0_124_6)">
            <path
              d="M8.61896 9.54685C8.49296 9.54685 8.36597 9.49988 8.26597 9.39988C8.07297 9.20688 8.07297 8.88685 8.26597 8.69285L11.959 4.99985L8.26597 1.30686C8.07297 1.11286 8.07297 0.792828 8.26597 0.599828C8.45897 0.406828 8.77897 0.406828 8.97297 0.599828L13.019 4.64683C13.213 4.83983 13.213 5.15988 13.019 5.35288L8.97297 9.39988C8.87297 9.49988 8.74596 9.54685 8.61896 9.54685Z"
              fill="#0053A5"
            />
            <path
              d="M12.554 5.5H1.33398C1.06098 5.5 0.833984 5.273 0.833984 5C0.833984 4.727 1.06098 4.5 1.33398 4.5H12.554C12.827 4.5 13.054 4.727 13.054 5C13.054 5.273 12.827 5.5 12.554 5.5Z"
              fill="#0053A5"
            />
          </g>
          <defs>
            <clipPath id="clip0_124_6">
              <rect width="14" height="10" fill="#0053A5" />
            </clipPath>
          </defs>
        </svg> */}
      </Button>
    </div>
  );
};
