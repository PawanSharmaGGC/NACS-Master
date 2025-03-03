import React, { useEffect } from "react";
import { Card } from "react-bootstrap";
import nacsLogo from "../../assests/images/nacs-logo.png";
import clip from "../../assests/images/Clip path group.png";
import FooterStyle from "../../stylesheets/FooterStyle.module.css";

export const Footer = () => {
  useEffect(() => {
    window.addEventListener("resize", () => {
      const zoomLevel = window.devicePixelRatio;
      const img = document.getElementById("clip_img");
      img.style.width = `${40 * zoomLevel}vw`;
    });
  });
  return (
    <div>
      <Card className={`bg-002569 ${FooterStyle.main_card}`}>
        <img
          id="clip_img"
          className={`${FooterStyle.clip_img} img-fluid`}
          src={clip}
          alt=""
        />
        <div className="p-4 p-lg-5 p-md-5 w-100">
          <div className="d-flex justify-content-between">
            <Card.Img
              variant="bottom"
              fluid
              src={nacsLogo}
              className={`mb-3 me-5 ${FooterStyle.logo}`}
            />
            <span
              className={`text-light fw-lighter text-start ms-5 ${FooterStyle.info}`}
            >
              NACS serves the global convenience and fuel retailing industry by
              providing industry knowledge, connections and issues leadership to
              ensure the competitive viability of its members’ businesses.
            </span>
          </div>
          <hr className={`${FooterStyle.divider} mb-lg-5`} />
          <Card.Body className={`text-start p-0`}>
            <div className={`${FooterStyle.footer_body}`}>
              <div className={`flex-grow-1 ${FooterStyle.main_card_body}`}>
                <div className="pe-lg-5">
                  <Card.Text className="mt-lg-1 mt-md-1 mt-3 text-white">
                    <p className="m-0 fs-5 fw-bold">NACS Headquarters</p>
                    <p className="m-0 mt-lg-2 fs-5 fw-light">
                      1600 Duke street
                    </p>
                    <p className="m-0 fs-5 fw-light">7th Floor</p>
                    <p className="m-0 fs-5 fw-light">Alexandria, VA 22314</p>
                  </Card.Text>
                  <Card.Text className="mt-lg-2 mt-md-2 mt-5 text-white">
                    <p className="m-0 fs-5 fw-bold">Contact</p>
                    <p className="m-0 mt-lg-2 fs-5 fw-light">
                      703-600-4986 (phone)
                    </p>
                    <p className="m-0 fs-5 fw-light">abc@sampledomain.com</p>
                  </Card.Text>
                </div>
                {/* Desktop screen lists */}

                <div
                  className={` flex-grow-1 mt-lg-1 mt-md-1 text-white ${FooterStyle.footer_links}`}
                >
                  <Card.Text>
                    <h6 className="m-0 fs-5 fw-bold">Quick Links</h6>
                    <ul className="text-start p-0 mt-3">
                      <li>
                        <a href="#">About</a>
                      </li>
                      <li>
                        <a href="#">Contact Us</a>
                      </li>
                      <li>
                        <a href="#">Careers</a>
                      </li>
                      <li>
                        <a href="#">Culture</a>
                      </li>
                      <li>
                        <a href="#">Articles</a>
                      </li>
                    </ul>
                  </Card.Text>
                  <Card.Text>
                    <h6 className="m-0 fs-5 fw-bold">Industry Partners</h6>
                    <ul className="text-start p-0 mt-3">
                      <li>
                        <a href="#">Conexxus</a>
                      </li>
                      <li>
                        <a href="#">Transportation Energy Institute</a>
                      </li>
                      <li>
                        <a href="#">Related Associations</a>
                      </li>
                    </ul>
                  </Card.Text>
                  <Card.Text>
                    <h6 className="m-0 fs-5 fw-bold">Stay Current</h6>
                    <ul className="text-start p-0 mt-3">
                      <li>
                        <a href="#">NACS Daily</a>
                      </li>
                      <li>
                        <a href="#">Cool New Products</a>
                      </li>
                      <li>
                        <a href="#">Fuel Market News</a>
                      </li>
                      <li>
                        <a href="#">NACS Magazine</a>
                      </li>
                      <li>
                        <a href="#">Retail Trends</a>
                      </li>
                    </ul>
                  </Card.Text>
                </div>

                {/* Mobile screen accordion */}

                <Card.Text>
                  <div
                    className={`accordion accordion-flush ${FooterStyle.footer_accordion}`}
                    id="accordionFlushExample"
                  >
                    <div
                      className={`accordion-item border_bottom_white pt-3 pb-3`}
                    >
                      <h2 className="accordion-header">
                        <button
                          className="accordion-button collapsed ps-0"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target="#flush-collapseOne"
                          aria-expanded="false"
                          aria-controls="flush-collapseOne"
                        >
                          Quick Links
                        </button>
                      </h2>
                      <div
                        id="flush-collapseOne"
                        className="accordion-collapse collapse"
                        data-bs-parent="#accordionFlushExample"
                      >
                        <div className="accordion-body">
                          Vivamus eu neque eget massa volutpat dignissim non a
                          diam. Nam et orci eget massa tempor imperdiet id eu
                          libero. Phasellus vel mi tellus.
                        </div>
                      </div>
                    </div>
                    <div className="accordion-item border_bottom_white pt-3 pb-3">
                      <h2 className="accordion-header">
                        <button
                          className="accordion-button collapsed ps-0"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target="#flush-collapseTwo"
                          aria-expanded="false"
                          aria-controls="flush-collapseTwo"
                        >
                          Industry Partners
                        </button>
                      </h2>
                      <div
                        id="flush-collapseTwo"
                        className="accordion-collapse collapse"
                        data-bs-parent="#accordionFlushExample"
                      >
                        <div className="accordion-body">
                          Duis fermentum eros ante, sed congue tortor tempus
                          lobortis. Nullam blandit, nisl eu rhoncus cursus,
                          dolor ex luctus leo, vel molestie ligula nibh id
                          mauris. Nullam ultrices diam at mi mattis bibendum.
                        </div>
                      </div>
                    </div>
                    <div className="accordion-item border_bottom_white pt-3 pb-3">
                      <h2 className="accordion-header">
                        <button
                          className="accordion-button collapsed ps-0"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target="#flush-collapseThree"
                          aria-expanded="false"
                          aria-controls="flush-collapseThree"
                        >
                          Stay Current
                        </button>
                      </h2>
                      <div
                        id="flush-collapseThree"
                        className="accordion-collapse collapse"
                        data-bs-parent="#accordionFlushExample"
                      >
                        <div className="accordion-body">
                          Aliquam dui sapien, ullamcorper suscipit euismod vel,
                          aliquam facilisis sapien. Fusce et lacinia dolor, et
                          tristique risus. Morbi venenatis quis velit et
                          placerat.
                        </div>
                      </div>
                    </div>
                  </div>
                </Card.Text>
              </div>
              <Card.Text>
                <div
                  className={`d-flex justify-content-between mt-lg-2 mb-lg-2 mt-4 mb-4 ${FooterStyle.footer_logo}`}
                >
                  <div className="m-auto ms-0">
                    <i className="fa-brands fa-facebook-f fa-xl color-FFFFFF pe-3"></i>
                    <i className="fa-brands fa-youtube fa-xl color-FFFFFF pe-3"></i>
                    <i className="fa-brands fa-instagram fa-xl color-FFFFFF pe-3"></i>
                    <i className="fa-brands fa-x-twitter fa-xl color-FFFFFF pe-3"></i>
                    <i className="fa-brands fa-linkedin-in fa-xl color-FFFFFF"></i>
                  </div>
                  <div className={`mt-lg-4 ${FooterStyle.back_to_btn}`}>
                    <i className="fa-light fa-arrow-up-from-arc color-FFFFFF"></i>
                    <span className="ps-2 fs-6 text-white">Back To top</span>
                  </div>
                  <div className={`mt-lg-4 ${FooterStyle.get_help_btn}`}>
                    <i className="fa-light fa-message-question color-FFFFFF"></i>
                    <span className="ps-2 fs-6 text-white">Get Help</span>
                  </div>
                </div>
              </Card.Text>
            </div>
          </Card.Body>
          <hr className={`${FooterStyle.divider}`} />
          <Card.Body>
            <Card.Text className={`text-white ${FooterStyle.bottom_section}`}>
              <p className="m-0 mb-1">
                <i className="fa-regular fa-copyright color-FFFFFF"></i>
                <span className="ps-2">NACS ALL RIGHTS RESERVED</span>
              </p>
              <p className="m-0 mb-1">Privacy Policy | Terms of Use</p>
              <p className="m-0 mb-1">Designed and Powered by Protiviti</p>
            </Card.Text>
          </Card.Body>
        </div>
      </Card>
    </div>
  );
};
