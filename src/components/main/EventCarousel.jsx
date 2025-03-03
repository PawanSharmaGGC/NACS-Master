import React, { useRef, useState } from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import EventCarouselStyle from "../../stylesheets/EventCarouselStyle.module.css";
import EventImage1 from "../../assests/images/event-carousel-1.png";
import EventImage2 from "../../assests/images/event-carousel-2.png";
import EventImage3 from "../../assests/images/event-carousel-3.png";
import { Card } from "react-bootstrap";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Tag } from "../ui-components/Tag";
import { PaginationDot } from "../ui-components/PaginationDot";

const data = [
  {
    id: 1,
    content: EventImage1,
    desc: "People enjoying 2024 NACS show",
  },
  {
    id: 2,
    content: EventImage2,
    desc: "People enjoying 2024 NACS show",
  },
  {
    id: 3,
    content: EventImage3,
    desc: "People enjoying 2024 NACS show",
  },
];

export const EventCarousel = () => {
  const settings = {
    className: "center",
    centerMode: true,
    infinite: true,
    centerPadding: "10%",
    slidesToShow: 1,
    speed: 500,
  };
  let containerRef = useRef(null);
  const [currElement, setCurrElement] = useState(0);
  const [prevElement, setPrevElement] = useState(data.length - 1);
  const [nextElement, setNextElement] = useState(1);

  const next = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickNext();
    setCurrElement(currElement < data.length - 1 ? count + 1 : 0);
    setPrevElement(currElement < data.length - 1 ? count : data.length - 1);
    setNextElement(
      count + 1 == data.length - 1
        ? 0
        : count == data.length - 1
        ? 1
        : count + 2
    );
  };
  const previous = () => {
    let count = containerRef.innerSlider.track.props.currentSlide;
    containerRef.slickPrev();
    setCurrElement(currElement > 0 ? count - 1 : data.length - 1);
    setPrevElement(
      count == 0 ? data.length - 2 : count == 1 ? data.length - 1 : count - 2
    );
    setNextElement(count == 0 ? 0 : count);
  };
  return (
    <div>
      <Card className={`border-0 p-0`}>
        <Card className={`p-0 ${EventCarouselStyle.section_card}`}>
          <Card.Body className={`p-0`}>
            <div className="text-start mb-3 ms-2 me-2">
              <div className="d-flex justify-content-between">
                <EyebrowTitle
                  leftBorderColor="border_left_green"
                  title="FEATURED"
                  titleColor="color-0053A5"
                />
                <Tag
                  tagName="NEW"
                  bgColor="bg-DBEAB9"
                  textColor="color-0053A5"
                />
              </div>
              <p className="fs-4 color-002569 pt-3">
                Convenience Summit Asia 2024 Gallery
              </p>
            </div>
            <div className={`${EventCarouselStyle.slider_container}`}>
              <Slider
                {...settings}
                ref={(slider) => {
                  containerRef = slider;
                }}
              >
                {data.map((item, index) => (
                  <Card key={index} className="border border-0">
                    <Card.Body
                      className={`${EventCarouselStyle.slider_container_card_body}`}
                    >
                      <img
                        src={item.content}
                        className={`img-fluid`}
                        alt="carousel-image"
                      />
                      <p className="m-0 pt-4 text-muted fw-light float-start">
                        {item.desc}
                      </p>
                    </Card.Body>
                  </Card>
                ))}
              </Slider>
              <PaginationDot
                currElement={currElement}
                data={data}
                next={next}
                previous={previous}
                position="justify-content-lg-center"
              />
            </div>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
