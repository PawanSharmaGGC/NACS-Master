import React, { useState } from "react";
import EventSearchSortFilterStyle from "../../stylesheets/EventSearchSortFilter.module.css";
import { TagsCluster } from "./TagsCluster";

const badgeData = [
  "2024",
  "2025",
  "State",
  "Regional",
  "International",
  "NACS",
];

export const EventSearchSortFilter = () => {
  const [filterBagde, setFilterBadge] = useState([]);

  const handleFilter = (item) => {
    let newArr = filterBagde;
    if (filterBagde.includes(item)) {
      newArr = newArr.filter((el) => el !== item);
      setFilterBadge(newArr);
    } else {
      newArr.push(item);
    }
    setFilterBadge([...newArr]);
  };

  return (
    <div>
      <div className={`d-flex p-2 ${EventSearchSortFilterStyle.main_card}`}>
        <div
          className={`input-group mb-3 me-4 ${EventSearchSortFilterStyle.input_group}`}
        >
          <i className="fa-solid fa-magnifying-glass p-1 ps-3 mt-2 color-0053A5"></i>
          <input
            type="text"
            className={`form-control border border-0 ${EventSearchSortFilterStyle.input_search}`}
            placeholder="Search"
            aria-label="Search"
            aria-describedby="basic-addon2"
          />
          <span
            className="input-group-text border border-0 ps-5 pe-5 bg-0053A5 color-FFFFFF pointer"
            id="basic-addon2"
          >
            Search
          </span>
        </div>
        <div className="w-25">
          <select
            className={`form-select bg-f5f4f4 border border-0 ${EventSearchSortFilterStyle.select_box}`}
            aria-label="Default select example"
          >
            <option selected>Topic</option>
            <option value="1">One</option>
            <option value="2">Two</option>
            <option value="3">Three</option>
          </select>
        </div>
      </div>
      <div className="text-start ps-3">
        <TagsCluster
          data={badgeData}
          filterData={filterBagde}
          handleFilter={handleFilter}
        />
      </div>
    </div>
  );
};
