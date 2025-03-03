import React from "react";
import Tier2ContentCardStyle from "../../stylesheets/Tier2ContentCardStyle.module.css";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import tier1ContentImg from "../../assests/images/tier-1-content-img.png";

export const Tier2ContentCard = () => {
  return (
    <div>
      <Card
        className={`p-lg-4 p-md-4 p-sm-4 p-3 border border-0 ${Tier2ContentCardStyle.main_card}`}
      >
        <Card
          className={`border border-0 ${Tier2ContentCardStyle.section_card}`}
        >
          <div className={`d-flex ${Tier2ContentCardStyle.section_card_div}`}>
            {/* This image can be dynamic to make the component tier4 content card */}
            <img
              className={`img-fluid ${Tier2ContentCardStyle.section_card_img}`}
              src={tier1ContentImg}
              alt="tier1ContentImg"
            />
            <Card.Body className="p-0 mt-3 flex-grow-1">
              <div className="text-start">
                <p className="fs-5 mb-0 color-396a99">
                  The NACS Show brings together convenience and fuel retailing
                  industry.
                </p>
              </div>
              <div className="d-flex justify-content-between mt-4">
                <p className="mb-0 text-primary text-start">
                  <span className="text-secondary me-1 me-lg-3">
                    24 March 2024
                  </span>
                  <span className="text-body-tertiary">5 min Read</span>
                </p>
              </div>
            </Card.Body>
          </div>
          <div className={`mt-4`}>
            <BackToLog />
          </div>
        </Card>
      </Card>
    </div>
  );
};
