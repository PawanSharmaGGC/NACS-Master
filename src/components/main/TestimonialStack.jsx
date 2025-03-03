import React, { useEffect, useRef, useState } from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import TestimonialStackStyle from "../../stylesheets/TestimonialStackStyle.module.css";

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

export const TestimonialStack = () => {
  const containerRef = useRef(null);
  const [topElement, setTopElement] = useState(null);

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
      <Card
        className={`p-3 ps-lg-5 ps-md-5 ps-3 ${TestimonialStackStyle.main_card}`}
      >
        <div
          className={`${TestimonialStackStyle.container_div}`}
          ref={containerRef}
        >
          {data.map((item, index) => (
            <Card
              className={`border border-0 ref-card mb-3 ${TestimonialStackStyle.flex_card}`}
              id={index}
              key={index}
            >
              <Card.Body
                className={`p-0 ${
                  topElement == index
                    ? TestimonialStackStyle.active_slider_card
                    : TestimonialStackStyle.slider_card
                }`}
              >
                <div className="text-start p-4">
                  <div className="mb-2">
                    <i
                      className={`fa-solid fa-quote-left ${
                        topElement == index ? "color-0053A5" : "color-FFFFFF"
                      } fa-2xl`}
                    ></i>
                  </div>
                  <div
                    className={`fs-5 mb-2 ${
                      topElement == index ? "color-002569" : "color-FFFFFF"
                    }`}
                  >
                    {item.text}
                  </div>
                  <div
                    className={`${
                      topElement == index ? "color-000000" : "color-FFFFFF"
                    }`}
                  >
                    {item.writter}
                  </div>
                </div>
              </Card.Body>
            </Card>
          ))}
        </div>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
