import React from "react";
import SearchStyle from "../../stylesheets/SearchStyle.module.css";

export const SearchComponent = () => {
  return (
    <div>
      <div className={`d-flex p-2 ${SearchStyle.main_card}`}>
        <div className={`input-group mb-3 me-4 ${SearchStyle.input_group}`}>
          <i className="fa-solid fa-magnifying-glass p-1 ps-3 mt-2 color-0053A5"></i>
          <input
            type="text"
            className={`form-control border border-0 ${SearchStyle.input_search}`}
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
        <div className={`d-flex ${SearchStyle.dropdown_section}`}>
          <div className="w-50 me-3">
            <select
              className={`form-select bg-f5f4f4 border border-0 ${SearchStyle.select_box}`}
              aria-label="Default select example"
            >
              <option selected>Topic</option>
              <option value="1">One</option>
              <option value="2">Two</option>
              <option value="3">Three</option>
            </select>
          </div>
          <div className="w-50">
            <select
              className={`form-select bg-f5f4f4 border border-0 ${SearchStyle.select_box}`}
              aria-label="Default select example"
            >
              <option selected>Sort By</option>
              <option value="1">One</option>
              <option value="2">Two</option>
              <option value="3">Three</option>
            </select>
          </div>
        </div>
      </div>
    </div>
  );
};
