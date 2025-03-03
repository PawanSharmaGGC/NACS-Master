import React from "react";
import EyebrowTitleStyle from "../../stylesheets/EyebrowTitleStyle.module.css";

export const EyebrowTitle = ({
  leftBorderColor,
  title,
  titleColor = "text-primary",
}) => {
  return (
    <div>
      <div
        className={`text-start mb-4 ${leftBorderColor} ${EyebrowTitleStyle.eyebrow}`}
      >
        <span className={`ps-4 text-uppercase font-monospace ${titleColor}`}>
          {title}
        </span>
      </div>
    </div>
  );
};
