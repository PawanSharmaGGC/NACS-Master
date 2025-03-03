import React from "react";
import { BackToLog } from "../ui-components/BackToLog";
import { Card } from "react-bootstrap";
import clip from "../../assests/images/Clip path group.png";
import TextOnlyHeroStyle from "../../stylesheets/TextOnlyHeroStyle.module.css";
import { Breadcrumbs } from "../ui-components/Breadcrumbs";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { DateTimeArticleSummary } from "../ui-components/DateTimeArticleSummary";

const page = ["Home", "News"];

export const TextOnlyHero = () => {
  return (
    <div>
      <Card className={`p-lg-0 p-md-0 p-3 ${TextOnlyHeroStyle.main_card}`}>
        <img className={`${TextOnlyHeroStyle.clip_image}`} src={clip} alt="" />
        <Card className={`border-0 ${TextOnlyHeroStyle.section_card}`}>
          <Card.Body className="p-lg-5 p-md-5 p-0">
            <div className={`${TextOnlyHeroStyle.breadcrumb}`}>
              <Breadcrumbs pages={page} />
            </div>
            <EyebrowTitle
              leftBorderColor="border_left_green"
              title="NACS DAILY NEWS"
            />
            <div className=" text-start mb-3 mt-lg-5">
              <span className="color-002569 fw-semibold fs-3">
                Stopping Swipe Fees On State Taxes: Illinois's Landmark Law
              </span>
              <p className={`mt-3 fs-6 ${TextOnlyHeroStyle.text_details}`}>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                Phasellus massa diam, ullamcorper eu odio at, suscipit tristique
                magna. Sed vitae venenatis lacus. Aenean ullamcorper congue
                erat, non hendrerit orci maximus a. Cras id elit aliquet,
                gravida justo sed, molestie ex. Donec mollis erat a porttitor
                egestas. Nunc vel purus purus. Nullam viverra felis augue, sed
                hendrerit augue rhoncus nec. Maecenas in gravida purus, ut
                laoreet ex.
              </p>
            </div>
            <DateTimeArticleSummary
              textColor="color-262d61"
              date="24 March 2024"
              time="03:00 PM"
              article="Eastern Time"
              divider={true}
              dividerColor="color-868F98"
              fontSize="fs-5"
            />
          </Card.Body>
        </Card>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
