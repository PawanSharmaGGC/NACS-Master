import React from "react";
import { BackToLog } from "../ui-components/BackToLog";
import { Card } from "react-bootstrap";
import RelatedContentCardStyle from "../../stylesheets/RelatedContentCardStyle.module.css";
import CLogoGreen from "../../assests/images/c-logo-green.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const RelatedContentCard = () => {
  return (
    <div>
      <Card className={`p-3 border-0 ${RelatedContentCardStyle.main_card}`}>
        <Card className={`border-0 ${RelatedContentCardStyle.section_card}`}>
          <Card.Body className="p-4">
            <EyebrowTitle
              leftBorderColor="border_left_blue"
              title="NACS DAILY NEWS"
            />
            <div className="p-1 text-start">
              <span className="color-0053A5">
                10 Convinient Store Facts That WIll Make You Rethink Your
                Current Business Methods
              </span>
            </div>
            <div className="d-flex justify-content-between">
              <div className="fs-5 mt-3">
                <span className="me-2 color-0053A5">Read Story</span>
                <i className={`fa-regular fa-arrow-right color-0053A5`}></i>
              </div>
              <div className="align-self-end opacity-50">
                <img src={CLogoGreen} alt="" />
              </div>
            </div>
          </Card.Body>
        </Card>
      </Card>
      <div className="mt-5">
        <BackToLog />
      </div>
    </div>
  );
};
