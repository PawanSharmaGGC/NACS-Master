import React from "react";
import ContentFeedFilterStyle from "../../stylesheets/ContentFeedFilter.module.css";

export const ContentFeedFilter = () => {
  return (
    <div>
      <div
        className={`d-flex m-2 justify-content-between ${ContentFeedFilterStyle.main_div}`}
      >
        <div className="fs-4 color-0053A5">From This Series</div>
        <div className="w-25">
          <select
            className={`form-select ${ContentFeedFilterStyle.content_select_box}`}
            aria-label="Default select example"
          >
            <option selected>Type</option>
            <option value="1">One</option>
            <option value="2">Two</option>
            <option value="3">Three</option>
          </select>
        </div>
      </div>
    </div>
  );
};
