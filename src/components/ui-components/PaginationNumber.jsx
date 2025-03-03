import React from "react";
import PaginationNumberStyle from "../../stylesheets/PaginationNumber.module.css";

export const PaginationNumber = ({
  data,
  activePage = 1,
  next,
  previous,
  setActivePage,
}) => {
  const totalPages = Math.ceil(data.length / 10);
  const generatePageNumbers = () => {
    let currentPage = activePage;
    const pages = [];
    const maxVisiblePages = 5;
    if (totalPages <= maxVisiblePages) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      if (currentPage > maxVisiblePages - 2) {
        pages.push("...");
      }
      if (currentPage - 1 > 1 && currentPage + 1 < totalPages) {
        pages.push(currentPage - 1, currentPage, currentPage + 1);
      } else if (currentPage - 1 <= 1) {
        pages.push(currentPage, currentPage + 1);
      } else if (currentPage + 1 >= totalPages) {
        pages.push(currentPage - 1, currentPage);
      }
      if (currentPage < totalPages - 1) {
        pages.push("...");
      }
      if (!pages.includes(totalPages)) {
        pages.push(totalPages);
      }
    }
    return pages;
  };

  const pageNumbers = generatePageNumbers();

  return (
    <div>
      <nav aria-label="Page navigation example">
        <ul className="pagination justify-content-center">
          <li className="page-item">
            <a
              className={`page-link ${PaginationNumberStyle.page_link_button} ${
                activePage === 1 ? "disableItem" : ""
              }`}
              href="#"
              onClick={previous}
            >
              <i className="fa-regular fa-angle-left fa-xl"></i>
              <span className="ps-2">Prev</span>
            </a>
          </li>
          {pageNumbers.map((pageNumber, index) => (
            <li className="page-item" key={index}>
              <a
                className={`page-link ${
                  pageNumber === activePage
                    ? PaginationNumberStyle.active_page
                    : ""
                } ${PaginationNumberStyle.page_link}`}
                href="#"
                onClick={() => {
                  if (pageNumber !== "...") setActivePage(pageNumber);
                }}
              >
                {pageNumber}
              </a>
            </li>
          ))}

          <li className="page-item">
            <a
              className={`page-link ${PaginationNumberStyle.page_link_button} ${
                activePage === totalPages ? "disableItem" : ""
              }`}
              href="#"
              onClick={activePage < totalPages ? next : null}
            >
              <span className="pe-2">Next</span>
              <i className="fa-regular fa-angle-right fa-xl "></i>
            </a>
          </li>
        </ul>
      </nav>
    </div>
  );
};
