import React from "react";
import { Card } from "react-bootstrap";
import EventDetailsStyle from "../../stylesheets/EventDetailsStyle.module.css";
import { CountdownTimer } from "../ui-components/CountdownTimer";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const EventDetails = () => {
  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card className={`p-0 ${EventDetailsStyle.section_card}`}>
          <Card.Body className={`p-0`}>
            <EyebrowTitle
              leftBorderColor="border_left_green"
              title="event details"
              titleColor="color-0053A5"
            />
            <div className={`d-flex ${EventDetailsStyle.section_body_card}`}>
              <div
                className={`flex-fill pe-lg-4 pb-lg-0 pb-4 text-start mt-3 mt-lg-5 ${EventDetailsStyle.section_body_card_brdr}`}
              >
                <p className="fs-4 color-3C5567 mb-3">Time & Date:</p>
                <p className="d-flex flex-column mb-4 fs-5">
                  <div>
                    <i className="fa-solid fa-clock color-62BD19 pe-3"></i>
                    <span className="pe-3 fw-lighter color-3C5567">
                      8:00am - 12:00pm (Pacific)
                    </span>
                  </div>
                  <div>
                    <i className="fa-solid fa-calendar color-62BD19 pe-3"></i>
                    <span className="pe-3 fw-lighter color-3C5567">
                      October 7, 2024
                    </span>
                  </div>
                </p>
              </div>
              <div
                className={`flex-fill ps-lg-5 pe-lg-4 pt-lg-0 pt-1 pb-lg-0 pb-4 text-start mt-5 ${EventDetailsStyle.section_body_card_brdr}`}
              >
                <p className="fs-4 color-3C5567 mb-4 ">Location:</p>
                <p className="d-flex flex-column mb-4 fs-5">
                  <div>
                    <i className="fa-solid fa-right-left-large color-62BD19 pe-3"></i>
                    <span className="pe-3 fw-lighter color-3C5567">
                      Pavilion 9, Westgate Las Vegas
                    </span>
                  </div>
                  <div>
                    <i className="fa-solid fa-location-dot color-62BD19 pe-3"></i>
                    <span className="pe-3 fw-lighter color-3C5567">
                      NACS Show, Las Vegas, NV
                    </span>
                  </div>
                </p>
              </div>
              <CountdownTimer eventDate="11-15-2028" />
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
