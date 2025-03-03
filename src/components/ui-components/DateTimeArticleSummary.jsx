import React from "react";

export const DateTimeArticleSummary = ({
  textColor,
  date,
  time = "",
  article = "",
  fontSize,
  divider = false,
  dividerColor = "",
}) => {
  return (
    <div>
      <div className={`text-start ${textColor}`}>
        <p className={`m-0 ${fontSize}`}>
          <span>{date}</span>
          <span
            className={`${dividerColor} ${
              divider ? "ps-3 pe-3 fs-3" : "pe-1 ps-1"
            }`}
          >
            {divider && "|"}
          </span>
          <span className="text-body-tertiary">{time + " " + article}</span>
        </p>
      </div>
    </div>
  );
};
