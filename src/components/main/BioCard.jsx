import React, { useState } from "react";
import { Card } from "react-bootstrap";
import vector from "../../assests/images/Vector.png";
import { BackToLog } from "../ui-components/BackToLog";
import BioCardStyle from "../../stylesheets/BioCardStyle.module.css";
import { Avatars } from "../ui-components/Avatars";

export const BioCard = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleCollapse = () => {
    setIsOpen(!isOpen);
    const fCollapse = document.getElementById("first-collapse");
    if (!isOpen) {
      fCollapse.style.borderRadius = "0px 0px 0px 0px";
    } else {
      fCollapse.style.borderRadius = "0px 0px 18px 0px";
    }
  };

  return (
    <div>
      <Card
        className={`border border-0 p-lg-4 p-md-4 p-sm-4 p-3 ${BioCardStyle.main_card}`}
      >
        <Card className={`border ${BioCardStyle.section_card}`}>
          <Card.Body className="p-0">
            <div className="d-flex p-4">
              <div className="w-25">
                <img className="rounded-circle" src={vector} alt="" />
              </div>
              <div className="w-75 text-start ps-lg-3 ps-4">
                <p className="fs-5 fw-semibold mb-0 color-262d61">
                  Vanessa Weitzman - Brown
                </p>
                <p className="mb-0">Executive Director of Payments</p>
                <p className="mb-0 text-primary">+1 908-678-4564</p>
              </div>
            </div>
            <hr className="m-0" />
            <div className="d-flex">
              <div className="flex-fill fw-bold align-middle text-center fs-3 p-2 color-396a99">
                in
              </div>
              <div
                id="first-collapse"
                className={`text-light text-center align-middle fs-5 ${BioCardStyle.read_bio_btn} ${BioCardStyle.read_bio_btn_50}`}
                onClick={toggleCollapse}
                aria-expanded={isOpen}
              >
                {" "}
                Read Bio <span>{isOpen ? "-" : "+"}</span>
              </div>
            </div>
            <div
              className={`collapse ${isOpen ? "show" : ""}`}
              id="collapseExample"
            >
              <div
                className={`card card-body border border-2 bg-secondary-subtle text-start p-3 fs-5 ${BioCardStyle.collapse_body_content} `}
              >
                <div>
                  Contrary to popular belief, Lorem Ipsum is not simply random
                  text. It has roots in a piece of classical Latin literature
                  from 45 BC, making it over 2000 years old. Richard McClintock,
                  a Latin professor at Hampden-Sydney College in Virginia,
                  looked up one of the more obscure Latin words, consectetur,
                  from a Lorem Ipsum passage, and going through the cites of the
                  word in classical literature, discovered the undoubtable
                  source.
                </div>

                <div className="fs-5 mt-3">
                  <span className="me-2 color-0053A5">View Bio Page</span>
                  <i className="fa-regular fa-arrow-right color-0053A5"></i>
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>
        <div className="mb-4"></div>
        <Card className={`border ${BioCardStyle.section_card}`}>
          <Card.Body className="p-0">
            <div className="d-flex p-4">
              <div className="w-25">
                <Avatars
                  size={80}
                  imgSrc={vector}
                  // isName={true}
                  // name="Vanessa Weitzman"
                />
              </div>
              <div className="w-75 text-start ps-lg-3 ps-4">
                <p className="fs-5 fw-semibold mb-0 color-262d61">
                  Vanessa Weitzman - Brown
                </p>
                <p className="mb-0">Executive Director of Payments</p>
                <p className="mb-0 text-primary">+1 908-678-4564</p>
              </div>
            </div>
            <hr className="m-0" />
            <div className="d-flex">
              <div
                className={`text-light text-center align-middle fs-5 ${BioCardStyle.read_bio_btn} ${BioCardStyle.read_bio_btn_100}`}
              >
                {" "}
                Read Bio +
              </div>
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
