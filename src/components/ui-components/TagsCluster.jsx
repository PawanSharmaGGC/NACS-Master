import React from "react";

export const TagsCluster = ({ data, filterData, handleFilter }) => {
  return (
    <div>
      {data.map((item, index) => (
        <span
          id={index}
          className={`badge rounded-pill ${
            filterData.includes(item) ? "active_filter_badge" : "filter_badge"
          } `}
          onClick={() => handleFilter(item)}
        >
          {item}
        </span>
      ))}
    </div>
  );
};
