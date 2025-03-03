import React from "react";
import ExternalSiteStyle from "../../stylesheets/ExternalSiteCardStyle.module.css";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import ExpandableCard from "../../assests/images/expandable-card-img.jpeg";
import { InvertedButton } from "../ui-components/InvertedButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Tag } from "../ui-components/Tag";

export const ExternalSiteCard = () => {
  return (
    <div>
      <Card className={`border-0 p-3`}>
        <Card
          className={`border-0 p-0 bg-trans ${ExternalSiteStyle.section_card}`}
        >
          <Card.Body className={`p-4 ${ExternalSiteStyle.section_body_card}`}>
            <div className="text-start d-flex justify-content-between align-items-baseline">
              <EyebrowTitle
                leftBorderColor="border_left_green"
                title="THRIVR"
                titleColor="color-FFFFFF"
              />
              <Tag
                tagName="External Site"
                bgColor="bg-FFFFFF"
                textColor="color-0053A5"
              />
            </div>
            <div className=" pt-3 fs-3 color-FFFFFF text-start">
              What happens when consumers search for your business? do you know
              what shows up—or, worse, what doesn’t?
            </div>
            <div className="float-start pt-5">
              <InvertedButton
                name="Learn More About THRIVR"
                siconType="fa-regular"
                siconName="fa-arrow-up-right-from-square"
                siconColor="color-0053A5"
              />
            </div>
          </Card.Body>
          <Card.Body className={`p-0 `}>
            <img
              src={ExpandableCard}
              className={`${ExternalSiteStyle.section_body_img}`}
              alt=""
            />
          </Card.Body>
        </Card>
        <div className="mt-3 mb-3">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
