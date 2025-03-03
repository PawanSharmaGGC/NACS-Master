import React from "react";
import CompanyCardStyle from "../../stylesheets/CompanyCardStyle.module.css";
import { Card } from "react-bootstrap";
import CompanyImage from "../../assests/images/company-card.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const CompanyCard = () => {
  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card
          className={`border-0 p-0 bg-E9F0F4 ${CompanyCardStyle.section_card}`}
        >
          <Card.Body className={`p-4 ${CompanyCardStyle.section_body_card}`}>
            <div className="text-start mb-4 d-flex justify-content-between">
              <EyebrowTitle
                leftBorderColor="border_left_green"
                title="DESKTOP EYEBROW"
              />
              <img
                src={CompanyImage}
                className={`${CompanyCardStyle.section_body_img}`}
                alt="company-image"
              />
            </div>
            <div className="text-start">
              <p className="m-0 mb-3 fw-semibold fs-5 color-262d61">
                The Good Patch
              </p>
              <p className="m-0 mb-3 fs-5 color-262d61 mb-2">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua.
              </p>
              <p className="m-0 mb-3 ps-2 d-flex">
                <span className="pe-2">
                  <i className="fa-solid fa-location-dot color-396a99"></i>
                </span>
                <span className="pe-2 color-396a99">
                  100 Interstate Drive North Jasper, Georgia 30143, United
                  States of America
                </span>
              </p>
              <p className="m-0 mb-3 ps-2">
                <span className="pe-2">
                  <i className="fa-regular fa-link-simple color-396a99"></i>
                </span>
                <span className="pe-2 color-396a99">www.thegoodpatch.com</span>
              </p>
              <p className="m-0 mb-3 ps-2">
                <span className="pe-2">
                  <i className="fa-solid fa-circle-user color-396a99"></i>
                </span>
                <span className="pe-2 color-396a99">abc@xyz.com</span>
              </p>
              <p className="m-0 mb-3 ps-2">
                <span className="pe-2">
                  <i className="fa-solid fa-phone color-396a99"></i>
                </span>
                <span className="pe-2 color-396a99">1234556789</span>
              </p>
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
