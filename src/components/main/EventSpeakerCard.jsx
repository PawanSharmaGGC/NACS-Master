import React from "react";
import { Card } from "react-bootstrap";
import Speaker from "../../assests/images/speaker.png";
import { BackToLog } from "../ui-components/BackToLog";
import EventSpeakerCardStyle from "../../stylesheets/EventSpeakerCardStyle.module.css";

export const EventSpeakerCard = () => {
  return (
    <div>
      <Card
        className={`p-lg-4 p-md-4 p-sm-4 p-3 ${EventSpeakerCardStyle.main_card}`}
      >
        <Card
          className={`border border-secondary-subtle p-4 ${EventSpeakerCardStyle.section_card}`}
        >
          <Card.Img variant="top" src={Speaker} />
          <Card.Body className="d-flex justify-content-between">
            <div className="w-75 text-start ps-3">
              <p className="fs-5 fw-semibold mb-0 color-262d61">
                Sarah Friedrich
              </p>
              <p className="mb-0">Food Protechtion Analyst</p>
              <p className="mb-0">Kwik Trip Inc.</p>
            </div>
            <div className="align-self-end">
              <i className="fa-regular fa-arrow-right color-0053A5 fa-lg"></i>
            </div>
          </Card.Body>
        </Card>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
