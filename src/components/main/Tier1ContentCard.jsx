import React from "react";
import { BackToLog } from "../ui-components/BackToLog";
import Tier1ContentCardStyle from "../../stylesheets/Tier1ContentCardStyle.module.css";
import { Card } from "react-bootstrap";
import tier1ContentImg from "../../assests/images/tier-1-content-img.png";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { InvertedButton } from "../ui-components/InvertedButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Tag } from "../ui-components/Tag";
import { DateTimeArticleSummary } from "../ui-components/DateTimeArticleSummary";

export const Tier1ContentCard = () => {
  return (
    <div>
      <Card
        className={`p-0 border border-0 ${Tier1ContentCardStyle.main_card}`}
      >
        <Card
          className={`border border-secondary-subtle p-4 ${Tier1ContentCardStyle.section_card}`}
        >
          <div className="text-start d-flex justify-content-between align-items-baseline">
            <EyebrowTitle title="Event" leftBorderColor="border_left_green" />
            <Tag tagName="NEW" bgColor="bg-DBEAB9" textColor="color-0053A5" />
          </div>
          <Card.Img variant="top" src={tier1ContentImg} />
          <Card.Body className="p-0 mt-3">
            <div className="text-start">
              <h4 className="color-262d61 mb-3">NACS Show</h4>
              <p className="fs-6 mb-0 fw-semibold">
                The NACS Show brings together convenience and fuel retailing
                industry professionals for four days of learning, buying and
                selling, networking and fun — all designed to help participants
                grow their bottom line.
              </p>
            </div>
            <div className="pt-5">
              <DateTimeArticleSummary
                textColor="color-262d61"
                date="24 March 2024"
                article="Las Vegas, Nevada"
                divider={true}
                dividerColor="color-868F98"
                fontSize="fs-5"
              />
            </div>
            <div className={`mt-4 d-flex ${Tier1ContentCardStyle.btn_section}`}>
              <div className="mt-3 float-start">
                <SideRoundButton
                  name={"Register Now"}
                  siconName="fa-arrow-right-long"
                  btnClass="w-100 d-inline-block"
                />
              </div>
              <div className="mt-3 float-end">
                <InvertedButton
                  name="Add To Calender"
                  siconType="fa-solid"
                  siconName="fa-square-plus"
                  siconColor="color-0053A5"
                  siconSize="fa-lg"
                  btnClass="w-100 mt-lg-0 mt-22 d-inline-block"
                />
              </div>
            </div>
          </Card.Body>
        </Card>
        {/* <div className="mt-5">
          <BackToLog />
        </div> */}
      </Card>
    </div>
  );
};
