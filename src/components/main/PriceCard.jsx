import React from "react";
import { Card } from "react-bootstrap";
import PriceCardStyle from "../../stylesheets/PriceCardStyle.module.css";
import { InvertedButton } from "../ui-components/InvertedButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const PriceCard = () => {
  return (
    <div>
      <Card
        className={`border border-secondary-subtle p-4 ${PriceCardStyle.section_card}`}
      >
        <Card.Body className="color-FFFFFF">
          <EyebrowTitle
            leftBorderColor="border_left_green"
            title="2024"
            titleColor="color-FFFFFF"
          />
          <div className="text-start">
            <p className="fs-4 fw-semibold m-0 mb-0">Pricing & Registration</p>
            <p className="m-0 mb-3 fw-light opacity-75">
              This year's Food Safety Forum is co-located at the NACS Show and
              requires a separate registration.
            </p>
            <p className="m-0 mb-3 d-flex w-75">
              <span className="fs-2 fw-semibold pe-3">$329</span>
              <span className="fs-6 fw-light opacity-75">
                For NACS Retail Members
              </span>
            </p>
            <p className="m-0 mb-3">
              <div className="fs-5">Others:</div>
              <div className="d-flex">
                <span className="fs-5 fw-semibold pe-3">$658</span>
                <span className="fw-light opacity-75">
                  for retail non-members
                </span>
              </div>
              <div className="d-flex">
                <span className="fs-5 fw-semibold pe-3">$658</span>
                <span className="fw-light opacity-75">
                  for retail non-members
                </span>
              </div>
              <div className="d-flex">
                <span className="fs-5 fw-semibold pe-3">$658</span>
                <span className="fw-light opacity-75">
                  for retail non-members
                </span>
              </div>
            </p>
            <div>
              <InvertedButton
                name="Register Now"
                ficonType="fa-solid"
                ficonName="fa-circle-dollar"
                ficonColor="color-0053A5"
                siconType="fa-regular"
                siconName="fa-arrow-right"
                siconColor="color-0053A5"
                className="text-center"
              />
            </div>
          </div>
        </Card.Body>
      </Card>
    </div>
  );
};
