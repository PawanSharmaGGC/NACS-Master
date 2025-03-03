import React from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import PullQuoteStyle from "../../stylesheets/PullQuoteStyle.module.css";

export const PullQuote = () => {
  return (
    <div>
      <Card className={`p-3 ${PullQuoteStyle.main_card}`}>
        <div className="d-flex flex-column overflow-auto">
          <Card className="flex-card border border-0 mb-3">
            <Card.Body className={`p-0 ${PullQuoteStyle.section_card_body}`}>
              <div className="text-start p-4">
                <div className="mb-2">
                  <i className="fa-solid fa-quote-left color-0053A5 fa-2xl"></i>
                </div>
                <div className="fs-5 mb-2 color-002569">
                  The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody
                  Else Up.
                </div>
                <div className="color-000000">Mercy W.</div>
              </div>
            </Card.Body>
          </Card>
        </div>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
