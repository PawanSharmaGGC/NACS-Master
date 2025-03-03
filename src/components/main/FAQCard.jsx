import React, { useState } from "react";
import { Card } from "react-bootstrap";
import FAQCardStyle from "../../stylesheets/FAQCardStyle.module.css";

const data = [
  {
    id: 1,
    title: "How often do people charge their EVs?",
    desc: "1) Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
  },
  {
    id: 2,
    title: "Should I up-charge for charging?",
    desc: "2) Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
  },
  {
    id: 3,
    title: "Who is the ebst provider for EV charger?",
    desc: "3) Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
  },
];

export const FAQCard = () => {
  const [isOpen, setIsOpen] = useState(-1);

  const toggleCollapse = (id) => {
    if (isOpen === id) {
      setIsOpen(-1);
    } else {
      setIsOpen(id);
    }
  };
  return (
    <div>
      <Card className="border border-0">
        <Card.Text className="text-start color-0053A5">FAQS</Card.Text>
        <Card.Body>
          {data.map((item, index) => (
            <>
              <div
                id={`first_collapse_${index}`}
                className={`text-light text-center align-middle fs-5 bg-F5F5F5 brdr-rad-18 p-3 ps-4 mb-4`}
                onClick={() => toggleCollapse(item.id)}
                aria-expanded={isOpen}
              >
                <div className="d-flex justify-content-between">
                  <span className="d-inline-block color-0053A5 text-start pt-1">
                    {item.title}
                  </span>
                  <span
                    className={`bg-0053A5 w-10 brdr-rad-18 pb-1 fs-4 pointer ${FAQCardStyle.faq_btn}`}
                  >
                    {isOpen === item.id ? "-" : "+"}
                  </span>
                </div>
              </div>
              <div
                className={`collapse ${isOpen === item.id ? "show" : ""} mb-2`}
                id="collapseExample"
              >
                <div
                  className={`card card-body brdr-rad-18 border border-0 bg-secondary-subtle text-start p-3 fs-5`}
                >
                  <div>{item.desc}</div>
                </div>
              </div>
            </>
          ))}
        </Card.Body>
      </Card>
    </div>
  );
};
