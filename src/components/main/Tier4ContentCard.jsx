import React from "react";
import { BackToLog } from "../ui-components/BackToLog";
import { Card } from "react-bootstrap";
import Tier4ContentCardStyle from "../../stylesheets/Tier4ContentCard.module.css";

export const Tier4ContentCard = () => {
  return (
    <div>
      <Card className={`p-3 border-0 ${Tier4ContentCardStyle.main_card}`}>
        <Card className={`border-0 ${Tier4ContentCardStyle.section_card}`}>
          <Card.Body className="p-4">
            <div className="p-1 text-start fw-semibold">
              <span className="color-0053A5">
                10 Convinient Store Facts That WIll Make You Rethink Your
                Current Business Methods
              </span>
            </div>
            <div className="d-flex justify-content-between">
              <p className="mb-0 text-primary">
                <span className="text-secondary me-3">24 March 2024</span>
                <span className="text-body-tertiary">10 min Listen</span>
              </p>
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
