import React, { useEffect, useRef, useState } from "react";
import Tier3ContentCardStyle from "../../stylesheets/Tier3ContentCard.module.css";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import CLogoGreen from "../../assests/images/c-logo-green.png";
import CLogoWhite from "../../assests/images/c-logo-white.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

const data = [
  {
    id: 1,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 2,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 3,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 4,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 5,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 6,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 7,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 8,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
  {
    id: 9,
    text: "The Best Way To Cheer Yourself Up Is To Try To Cheer Somebody Else Up.",
    writter: "Mercy W.",
  },
];

export const Tier3ContentCard = () => {
  const containerRef = useRef(null);
  const [topElement, setTopElement] = useState(0);

  const getTopElement = () => {
    const container = containerRef.current;
    const items = container?.querySelectorAll(".ref-card");
    const containerRect = container?.getBoundingClientRect();

    if (items?.length) {
      for (const item of items) {
        const itemRect = item.getBoundingClientRect();

        // Check if the top of the item is within the visible area of the container
        if (container.scrollLeft === 0) {
          if (
            itemRect.top >= containerRect.top &&
            itemRect.top < containerRect.bottom
          ) {
            return item.id; // Return the first visible item's text
          }
        } else {
          // 400 is card width provided in css
          return Math.floor(container.scrollLeft / 400);
        }
      }
    }
    return null;
  };

  useEffect(() => {
    const handleScroll = () => {
      console.log("calling...");
      const topVisibleElement = getTopElement();
      setTopElement(topVisibleElement);
    };

    const container = containerRef.current;
    container?.addEventListener("scroll", handleScroll);

    // Initial check on mount
    handleScroll();

    return () => {
      container?.removeEventListener("scroll", handleScroll);
    };
  }, []);
  return (
    <div>
      <Card className={`p-3 ${Tier3ContentCardStyle.main_card}`}>
        <div
          className={`${Tier3ContentCardStyle.container_div}`}
          ref={containerRef}
        >
          {data.map((item, index) => (
            <Card
              className={`border border-0 ref-card mb-3 ${Tier3ContentCardStyle.flex_card}`}
              id={index}
              key={index}
            >
              <Card.Body
                className={`p-0 ${
                  Number(topElement) === Number(index)
                    ? Tier3ContentCardStyle.active_slider_card
                    : Tier3ContentCardStyle.slider_card
                }`}
              >
                <div className="text-start p-4">
                  <EyebrowTitle
                    title="Member News"
                    leftBorderColor={
                      Number(topElement) === Number(index)
                        ? "border_left_white bg-grad-dark"
                        : "border_left_green bg-grad-light"
                    }
                    titleColor={
                      Number(topElement) === Number(index)
                        ? "color-FFFFFF"
                        : "color-0053A5"
                    }
                  />
                  <div
                    className={`fs-5 mb-2 ${
                      Number(topElement) === Number(index)
                        ? "color-FFFFFF"
                        : "color-0053A5"
                    }`}
                  >
                    {item.text}
                  </div>
                  <div className={`d-flex justify-content-between `}>
                    <div>
                      <span>
                        <i
                          className={`fa-solid fa-lock-keyhole pe-2 ${
                            Number(topElement) === Number(index)
                              ? "color-FFFFFF"
                              : "color-0053A5"
                          }`}
                        ></i>
                      </span>
                      <span
                        className={`${
                          Number(topElement) === Number(index)
                            ? "color-FFFFFF"
                            : "color-0053A5"
                        }`}
                      >
                        Read Story
                      </span>
                      <span>
                        <i
                          className={`fa-regular fa-arrow-right ps-2 ${
                            Number(topElement) === Number(index)
                              ? "color-FFFFFF"
                              : "color-0053A5"
                          }`}
                        ></i>
                      </span>
                    </div>
                    <div>
                      <img
                        src={
                          Number(topElement) === Number(index)
                            ? CLogoWhite
                            : CLogoGreen
                        }
                        alt=""
                      />
                    </div>
                  </div>
                </div>
              </Card.Body>
            </Card>
          ))}
        </div>
        <div className="mt-2">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
