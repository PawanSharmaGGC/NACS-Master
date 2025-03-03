import React from "react";
import { Card } from "react-bootstrap";
import CTACardNoImageStyle from "../../stylesheets/CTACardNoImageStyle.module.css";
import { InvertedButton } from "../ui-components/InvertedButton";
import HalftonePattern from "../../assests/images/halftone-pattern.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const CTACardNoImage = () => {
  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card
          className={`border-0 p-0 bg-trans ${CTACardNoImageStyle.section_card}`}
        >
          <img
            className={`position-absolute end-0 opacity-25 z-1 ${CTACardNoImageStyle.halftone_img}`}
            src={HalftonePattern}
            alt="halftone-pattern"
          />
          <Card.Body className={`p-4 ${CTACardNoImageStyle.section_body_card}`}>
            <EyebrowTitle
              leftBorderColor="border_left_white"
              title="THRIVR"
              titleColor="color-FFFFFF"
            />
            <div className="row">
              <div className="col-lg-7 col-sm-12 col-12 pt-3 fs-3 color-FFFFFF text-start">
                What happens when consumers search for your business?
              </div>
              <div className="col-lg-5 col-sm-12 col-12 pt-4 pt-lg-1">
                <InvertedButton
                  name="Take Action Now"
                  siconType="fa-regular"
                  siconName="fa-arrow-right"
                  siconColor="color-0053A5"
                  className="float-lg-end float-start"
                />
              </div>
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
