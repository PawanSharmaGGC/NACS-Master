import React from "react";
import TagStyle from "../../stylesheets/TagsStyle.module.css";

export const Tag = ({ tagName, bgColor, textColor }) => {
  return (
    <span
      className={`text-center mt-2 ${textColor} ${bgColor} ${TagStyle.card_badge}`}
    >
      {tagName}
    </span>
  );
};
