import React, { useEffect, useState } from "react";
import HalftonePattern from "../../assests/images/halftone-pattern.png";
import MembershipCardStyle from "../../stylesheets/MembershipCardStyle.module.css";
import { Card } from "react-bootstrap";
import { PaginationNumber } from "../ui-components/PaginationNumber";

export const MembershipCard = () => {
  const [page, setPage] = useState([]);
  const [activePage, setActivePage] = useState(1);

  useEffect(() => {
    const tempArr = [];
    for (let i = 1; i <= 100; i++) {
      tempArr.push(i);
    }
    setPage(tempArr);
  }, []);

  const next = () => {
    let count = activePage;
    if (count < page.length) {
      count += 1;
      setActivePage(count);
    }
  };
  const previous = () => {
    let count = activePage;
    if (count > 1) {
      count -= 1;
      setActivePage(count);
    }
  };

  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card
          className={`border-0 p-0 bg-trans ${MembershipCardStyle.section_card}`}
        >
          <img
            className={`position-absolute opacity-25 z-1 ${MembershipCardStyle.halftone_img}`}
            src={HalftonePattern}
            alt="halftone-pattern"
          />
          <Card.Body className={`p-4 ${MembershipCardStyle.section_body_card}`}>
            <div className="text-center color-FFFFFF fw-semibold">
              NACS Membership By The Numbers
            </div>
            <div className="d-flex justify-content-around">
              <div className="pt-3 w-25">
                <div className="fs-3 fw-semibold">
                  <span className="color-FFFFFF">22.5</span>
                  <span className="color-62BD19 fs-3">k</span>
                </div>
                <div className="color-FFFFFF">
                  NACS Members in the United States of America
                </div>
              </div>

              <div className="pt-3 w-25">
                <div className="fs-3 fw-semibold">
                  <span className="color-FFFFFF">99</span>
                  <span className="color-62BD19 fs-3">%</span>
                </div>
                <div className="color-FFFFFF">
                  Of NACS Members renew their membership every year
                </div>
              </div>

              <div className="pt-3 w-25">
                <div className="fs-3 fw-semibold">
                  <span className="color-FFFFFF">4.1</span>
                  <span className="color-62BD19 fs-3">m</span>
                </div>
                <div className="color-FFFFFF">
                  Pounds of food donated by NACS members across USA
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>
      </Card>

      {page.length && (
        <PaginationNumber
          data={page}
          activePage={activePage}
          next={next}
          previous={previous}
          setActivePage={setActivePage}
        />
      )}
    </div>
  );
};
