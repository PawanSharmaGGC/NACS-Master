import React, { useEffect, useState } from "react";
import NavbarStyle from "../../stylesheets/NavbarStyle.module.css";
import NacsLogo from "../../assests/images/NACS-Logo-nav.png";

export const NavbarComponent = ({ tempProps }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [btmSecId, setBtmSecId] = useState(0);
  const [responsiveToggleId, setResponsiveToggleId] = useState(0);
  const [isNavbarToggle, setIsNavbarToogle] = useState(false);

  useEffect(() => {
    tempProps = tempProps.sort((a, b) => a.title.localeCompare(b.title));
  }, [tempProps]);

  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  const toggleBottomDropdown = (id) => {
    if (id !== btmSecId) {
      setBtmSecId(id);
    } else {
      setBtmSecId(0);
    }
  };

  const responsiveToggle = (id) => {
    if (id !== responsiveToggleId) {
      setResponsiveToggleId(id);
    } else {
      setResponsiveToggleId(0);
    }
  };

  useEffect(() => {
    if (responsiveToggleId > 0) {
      console.log("calling...");
      const list = document.getElementById("small_screen_nav_menu");
      const list_div = document.getElementById("navbarSupprotedContent11");
      list.style.width = "20%";
      list_div.className = "d-flex";
    }
    if (responsiveToggleId === 0) {
      console.log("calling...");
      const list = document.getElementById("small_screen_nav_menu");
      const list_div = document.getElementById("navbarSupprotedContent11");
      list_div.className = "d-block";
      list_div.style.width = "100%";
      list.style.width = "100%";
    }
  }, [responsiveToggleId]);

  const navbarToogler = () => {
    setIsNavbarToogle(!isNavbarToggle);
  };

  return (
    <div>
      {/* Upper Section */}
      <div className={`${NavbarStyle.upper_sec}`}>
        <nav className="navbar navbar-expand-lg ps-3 pe-3">
          <div className="container-fluid color-0053A5">
            <a className="color-0053A5 fs-6 fw-semibold navbar-brand" href="#">
              NACS Show
            </a>
            <span className="fs-6 fw-light">October 7-10</span>
            <button
              className="navbar-toggler"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navbarSupportedContent"
              aria-controls="navbarSupportedContent"
              aria-expanded="false"
              aria-label="Toggle navigation"
            >
              <span className="navbar-toggler-icon"></span>
            </button>
            <div
              className="collapse navbar-collapse justify-content-end"
              id="navbarSupportedContent"
            >
              <ul
                className={`navbar-nav mb-2 mb-lg-0 ${NavbarStyle.upper_sec_list}`}
              >
                <li className="nav-item">
                  <a
                    className="color-0053A5 fw-light nav-link active"
                    aria-current="page"
                    href="#"
                  >
                    NACS Show
                  </a>
                </li>
                <li className="nav-item">
                  <a className="color-0053A5 fw-light nav-link" href="#">
                    Magazine
                  </a>
                </li>
                <li className="nav-item">
                  <a className="color-0053A5 fw-light nav-link" href="#">
                    Store
                  </a>
                </li>
                <li className="nav-item">
                  <a className="color-0053A5 fw-light nav-link" href="#">
                    About
                  </a>
                </li>
              </ul>
              <form className="d-flex" role="search">
                <div className={`input-group ${NavbarStyle.input_icons}`}>
                  <i
                    className={`color-0053A5 fa-solid fa-magnifying-glass position-absolute ${NavbarStyle.icon}`}
                  ></i>
                  <input
                    type="search"
                    className={`form-control brdr-l-rad-18 ${NavbarStyle.input_field}`}
                    aria-label="Recipient's username"
                    aria-describedby="basic-addon2"
                  />
                  <span
                    className={`bg-0053A5 color-FFFFFF input-group-text brdr-r-rad-18 ${NavbarStyle.search_btn}`}
                    id="basic-addon2"
                  >
                    Search
                  </span>
                </div>
              </form>
              <div
                className={`dropdown ps-3 pe-5 ${
                  isOpen ? NavbarStyle.show : "dropdown"
                } `}
              >
                <button
                  className={`btn bg-trans primary-rounded-brdr dropdown-toggle ${NavbarStyle.dropdown_icon}`}
                  type="button"
                  data-bs-toggle="dropdown"
                  aria-expanded="false"
                  onClick={toggleDropdown}
                >
                  <i className="color-0053A5 fa-regular fa-circle-user"></i>
                  <span className="color-0053A5 ps-2">John Doe</span>
                </button>
                <ul
                  className={`dropdown-menu bg-E9F0F4 color-0053A5 ${
                    NavbarStyle.dropdown_menu
                  } ${isOpen ? NavbarStyle.show : "dropdown-menu"}`}
                >
                  <li>
                    <a
                      className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.upper_nav_dropdown_menu_header}`}
                      href="#"
                    >
                      My NACS
                      <span className="ps-2">
                        <i className="color-0053A5 fa-regular fa-arrow-right"></i>
                      </span>
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Profile
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Subscriptions
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Email Preferences
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Purchased Content
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      Saved Products
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      Logout
                      <span className="ps-2">
                        <i className="color-0053A5 fa-regular fa-arrow-right"></i>
                      </span>
                    </a>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </nav>
      </div>
      {/* Bottom Section */}
      <div className={`${NavbarStyle.bottom_sec}`}>
        <nav className="navbar navbar-expand-lg">
          <div
            className={`container-fluid ps-0 pe-0 ${NavbarStyle.bottom_container}`}
          >
            <a className="navbar-brand ps-4" href="#">
              <img
                src={NacsLogo}
                fluid
                alt=""
                className={`${NavbarStyle.nacs_logo}`}
              />
            </a>

            {/*Start Login and search For mobile screen */}
            <div className={`${NavbarStyle.mobile_resonsive_search_login}`}>
              <form className="" role="search">
                <div className={`input-group ${NavbarStyle.input_icons}`}>
                  <i
                    className={`color-FFFFFF fa-solid fa-magnifying-glass position-absolute ${NavbarStyle.icon}`}
                  ></i>
                  <input
                    type="search"
                    className={`form-control color-FFFFFF brdr-l-rad-18 ${NavbarStyle.input_field}`}
                    aria-label="Recipient's username"
                    aria-describedby="basic-addon2"
                  />
                  <span
                    className={`bg-0053A5 color-FFFFFF input-group-text brdr-r-rad-18 ${NavbarStyle.search_btn}`}
                    id="basic-addon2"
                  >
                    Search
                  </span>
                </div>
              </form>
              <div
                className={`dropdown pb-2 ${
                  isOpen ? NavbarStyle.show : "dropdown"
                } `}
              >
                <button
                  className={`btn bg-trans border border-0 dropdown-toggle ${NavbarStyle.dropdown_icon}`}
                  type="button"
                  data-bs-toggle="dropdown"
                  aria-expanded="false"
                  onClick={toggleDropdown}
                >
                  <i className="color-FFFFFF fa-regular fa-circle-user"></i>
                </button>
                <ul
                  className={`dropdown-menu bg-E9F0F4 color-0053A5 ${
                    NavbarStyle.dropdown_menu
                  } ${isOpen ? NavbarStyle.show : "dropdown-menu"}`}
                >
                  <li>
                    <a
                      className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.upper_nav_dropdown_menu_header}`}
                      href="#"
                    >
                      My NACS
                      <span className="ps-2">
                        <i className="color-0053A5 fa-regular fa-arrow-right"></i>
                      </span>
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Profile
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Subscriptions
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Email Preferences
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      My Purchased Content
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      Saved Products
                    </a>
                  </li>
                  <li>
                    <a className="dropdown-item" href="#">
                      Logout
                      <span className="ps-2">
                        <i className="color-0053A5 fa-regular fa-arrow-right"></i>
                      </span>
                    </a>
                  </li>
                </ul>
              </div>
            </div>
            {/*End Login and search For mobile screen */}

            <button
              className="navbar-toggler"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navbarSupportedContent1"
              aria-controls="navbarSupportedContent"
              aria-expanded={isNavbarToggle ? "true" : "false"}
              aria-label="Toggle navigation"
              onClick={navbarToogler}
            >
              <i
                className={`fa-solid ${
                  isNavbarToggle ? "fa-x" : "fa-bars"
                } color-FFFFFF`}
              ></i>
            </button>
            <div
              className="collapse navbar-collapse"
              id="navbarSupportedContent1"
            >
              <div id="navbarSupprotedContent11">
                {/* Start Menu on small screen */}
                <ul
                  id="small_screen_nav_menu"
                  className={`d-lg-none d-xl-none navbar-nav text-start mb-lg-0 bg-002362 ${NavbarStyle.bottom_sec_list}`}
                >
                  <div className="d-flex">
                    <li className={`nav-item pt-4`}>
                      <a
                        className={`nav-link ${NavbarStyle.bottom_sec_dropdown} `}
                        href="#"
                        role="button"
                        aria-expanded="false"
                      >
                        <i
                          onClick={() => responsiveToggle(1)}
                          className="color-FFFFFF fa-solid fa-newspaper fa-lg pe-3"
                        ></i>
                        <span
                          className={`${
                            responsiveToggleId > 0 ? "d-none" : "d-inline"
                          }`}
                        >
                          Stay Current
                        </span>
                      </a>
                    </li>
                    <div
                      className={`${NavbarStyle.list_side_icon} ${
                        responsiveToggleId > 0 ? "d-none" : "d-inline"
                      }`}
                    >
                      <i
                        onClick={() => responsiveToggle(1)}
                        className={`color-FFFFFF float-end pe-5 pt-5 fa-regular fa-angle-right fa-xl `}
                      ></i>
                    </div>
                  </div>
                  <div className="d-flex">
                    <li className={`nav-item`}>
                      <a
                        className={`nav-link ${NavbarStyle.bottom_sec_dropdown} `}
                        href="#"
                        role="button"
                        aria-expanded="false"
                        onClick={() => responsiveToggle(2)}
                      >
                        <i className="color-FFFFFF fa-solid fa-handshake-simple fa-lg pe-3"></i>
                        <span
                          className={`${
                            responsiveToggleId > 0 ? "d-none" : "d-inline"
                          }`}
                        >
                          Attend & Learn
                        </span>
                      </a>
                    </li>
                    <div
                      className={`${NavbarStyle.list_side_icon} ${
                        responsiveToggleId > 0 ? "d-none" : "d-inline"
                      }`}
                    >
                      <i
                        onClick={() => responsiveToggle(2)}
                        className={`color-FFFFFF float-end pe-5 pt-5 fa-regular fa-angle-right fa-xl `}
                      ></i>
                    </div>
                  </div>

                  <div className="d-flex">
                    <li className={`nav-item`}>
                      <a
                        className={`nav-link ${NavbarStyle.bottom_sec_dropdown} `}
                        href="#"
                        role="button"
                        aria-expanded="false"
                        onClick={() => responsiveToggle(3)}
                      >
                        <i className="color-FFFFFF fa-solid fa-chart-column fa-lg pe-3"></i>{" "}
                        <span
                          className={`${
                            responsiveToggleId > 0 ? "d-none" : "d-inline"
                          }`}
                        >
                          Power Your Business
                        </span>
                      </a>
                    </li>
                    <div
                      className={`${NavbarStyle.list_side_icon} ${
                        responsiveToggleId > 0 ? "d-none" : "d-inline"
                      }`}
                    >
                      <i
                        onClick={() => responsiveToggle(3)}
                        className={`color-FFFFFF float-end pe-5 pt-5 fa-regular fa-angle-right fa-xl `}
                      ></i>
                    </div>
                  </div>

                  <div className="d-flex">
                    <li className={`nav-item`}>
                      <a
                        className={`nav-link ${NavbarStyle.bottom_sec_dropdown} `}
                        href="#"
                        role="button"
                        aria-expanded="false"
                        onClick={() => responsiveToggle(4)}
                      >
                        <i className="fa-solid fa-building-columns fa-lg pe-3"></i>
                        <span
                          className={`${
                            responsiveToggleId > 0 ? "d-none" : "d-inline"
                          }`}
                        >
                          Streaghten the Industry
                        </span>
                      </a>
                    </li>
                    <div
                      className={`${NavbarStyle.list_side_icon} ${
                        responsiveToggleId > 0 ? "d-none" : "d-inline"
                      }`}
                    >
                      <i
                        onClick={() => responsiveToggle(4)}
                        className={`color-FFFFFF float-end pe-5 pt-5 fa-regular fa-angle-right fa-xl `}
                      ></i>
                    </div>
                  </div>
                  <div className="d-flex">
                    <li className={`nav-item`}>
                      <a
                        className={`nav-link ${NavbarStyle.bottom_sec_dropdown}`}
                        href="#"
                        role="button"
                        aria-expanded="false"
                        onClick={() => responsiveToggle(5)}
                      >
                        <i className="color-FFFFFF pe-3 fa-solid fa-address-card fa-lg"></i>
                        <span
                          className={`${
                            responsiveToggleId > 0 ? "d-none" : "d-inline"
                          }`}
                        >
                          Membership
                        </span>
                      </a>
                    </li>
                    <div
                      className={`${NavbarStyle.list_side_icon} ${
                        responsiveToggleId > 0 ? "d-none" : "d-inline"
                      }`}
                    >
                      <i
                        onClick={() => responsiveToggle(5)}
                        className={`color-FFFFFF float-end pe-5 pt-5 fa-regular fa-angle-right fa-xl `}
                      ></i>
                    </div>
                  </div>
                </ul>
                <div
                  className={`d-sm-block d-md-none d-lg-none ${
                    NavbarStyle.mobile_sub_menu_div
                  } ${
                    responsiveToggleId > 0 ? "d-block flex-grow-1" : "d-none"
                  }`}
                >
                  <div
                    id="1"
                    className={`${
                      responsiveToggleId === 1 ? "d-block" : "d-none"
                    }`}
                  >
                    <div className="float-start p-3">
                      <i
                        onClick={() => responsiveToggle(1)}
                        className="color-FFFFFF fa-solid fa-arrow-turn-left fa-lg"
                      ></i>{" "}
                      <span className="text-white ps-3">Back</span>
                    </div>
                    <ul
                      className={`bg-trans color-FFFFFF p-4 ${NavbarStyle.sub_menu_list}`}
                    >
                      <li className="pb-3">
                        <a
                          className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.bottom_nav_dropdown_menu_header}`}
                          href="#"
                        >
                          <span className="color-FFFFFF float-start">
                            Stay Current
                          </span>
                          <span className="ps-2">
                            <i className="color-FFFFFF float-end fa-regular fa-arrow-right pt-1"></i>
                          </span>
                        </a>
                      </li>
                      <li className="row">
                        {tempProps.map((item, index) => (
                          <div className="col-6 w-50" key={index}>
                            <a
                              id={item.id}
                              href={item.url}
                              className="dropdown-item text-wrap"
                            >
                              {item.title}
                            </a>
                          </div>
                        ))}
                      </li>
                    </ul>
                  </div>
                  <div
                    id="2"
                    className={`${
                      responsiveToggleId === 2 ? "d-block" : "d-none"
                    }`}
                  >
                    <div className="float-start p-3">
                      <i
                        onClick={() => responsiveToggle(2)}
                        className="color-FFFFFF fa-solid fa-arrow-turn-left fa-lg"
                      ></i>{" "}
                      <span className="text-white ps-3">Back</span>
                    </div>
                    <ul
                      className={`bg-trans color-FFFFFF p-4 ${NavbarStyle.sub_menu_list}`}
                    >
                      <li className="pb-3">
                        <a
                          className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.bottom_nav_dropdown_menu_header}`}
                          href="#"
                        >
                          <span className="color-FFFFFF float-start">
                            Attend & Learn
                          </span>
                          <span className="ps-2">
                            <i className="color-FFFFFF float-end fa-regular fa-arrow-right pt-1"></i>
                          </span>
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </div>
                  <div
                    id="3"
                    className={`${
                      responsiveToggleId === 3 ? "d-block" : "d-none"
                    }`}
                  >
                    <div className="float-start p-3">
                      <i
                        onClick={() => responsiveToggle(3)}
                        className="color-FFFFFF fa-solid fa-arrow-turn-left fa-lg"
                      ></i>{" "}
                      <span className="text-white ps-3">Back</span>
                    </div>
                    <ul
                      className={`bg-trans color-FFFFFF p-4 ${NavbarStyle.sub_menu_list}`}
                    >
                      <li className="pb-3">
                        <a
                          className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.bottom_nav_dropdown_menu_header}`}
                          href="#"
                        >
                          <span className="color-FFFFFF float-start">
                            Power Your Business
                          </span>
                          <span className="ps-2">
                            <i className="color-FFFFFF float-end fa-regular fa-arrow-right pt-1"></i>
                          </span>
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </div>
                  <div
                    id="4"
                    className={`${
                      responsiveToggleId === 4 ? "d-block" : "d-none"
                    }`}
                  >
                    <div className="float-start p-3">
                      <i
                        onClick={() => responsiveToggle(4)}
                        className="color-FFFFFF fa-solid fa-arrow-turn-left fa-lg"
                      ></i>{" "}
                      <span className="text-white ps-3">Back</span>
                    </div>
                    <ul
                      className={`bg-trans color-FFFFFF p-4 ${NavbarStyle.sub_menu_list}`}
                    >
                      <li className="pb-3">
                        <a
                          className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.bottom_nav_dropdown_menu_header}`}
                          href="#"
                        >
                          <span className="color-FFFFFF float-start">
                            Streaghten the Industry
                          </span>
                          <span className="ps-2">
                            <i className="color-FFFFFF float-end fa-regular fa-arrow-right pt-1"></i>
                          </span>
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </div>
                  <div
                    id="5"
                    className={`${
                      responsiveToggleId === 5 ? "d-block" : "d-none"
                    }`}
                  >
                    <div className="float-start p-3">
                      <i
                        onClick={() => responsiveToggle(5)}
                        className="color-FFFFFF fa-solid fa-arrow-turn-left fa-lg"
                      ></i>{" "}
                      <span className="text-white ps-3">Back</span>
                    </div>
                    <ul
                      className={`bg-trans color-FFFFFF p-4 ${NavbarStyle.sub_menu_list}`}
                    >
                      <li className="pb-3">
                        <a
                          className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.bottom_nav_dropdown_menu_header}`}
                          href="#"
                        >
                          <span className="color-FFFFFF float-start">
                            Membership
                          </span>
                          <span className="ps-2">
                            <i className="color-FFFFFF float-end fa-regular fa-arrow-right pt-1"></i>
                          </span>
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                      <li className="">
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </div>
                </div>
                {/* End Menu on small screen */}

                {/* Start Menu on large screen */}
                <ul
                  className={`d-none d-sm-flex navbar-nav ps-5 mb-lg-0 ${NavbarStyle.bottom_sec_list}`}
                >
                  <li
                    className={`nav-item dropdown ${
                      btmSecId === 1 ? NavbarStyle.btm_show : "dropdown"
                    }`}
                  >
                    <a
                      className={`nav-link dropdown-toggle ${
                        NavbarStyle.bottom_sec_dropdown
                      } ${
                        btmSecId === 1 ? NavbarStyle.btm_sec_list_active : ""
                      }`}
                      href="#"
                      role="button"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      onClick={() => toggleBottomDropdown(1)}
                    >
                      Stay Current
                    </a>
                    <ul
                      className={`dropdown-menu bg-E9F0F4 color-0053A5 p-4 ${
                        NavbarStyle.bottom_sec_dropdown_menu
                      } ${btmSecId === 1 ? NavbarStyle.btm_show : ""}`}
                    >
                      <li className="pb-3">
                        <a
                          className={`dropdown-item fs-6 fw-semibold ${NavbarStyle.bottom_nav_dropdown_menu_header}`}
                          href="#"
                        >
                          Stay Current
                          <span className="ps-2">
                            <i className="color-0053A5 fa-regular fa-arrow-right"></i>
                          </span>
                        </a>
                      </li>
                      <li className="row">
                        {tempProps.map((item, index) => (
                          <div className="col-6 w-50" key={index}>
                            <a
                              id={item.id}
                              href={item.url}
                              className="dropdown-item text-wrap"
                            >
                              {item.title}
                            </a>
                          </div>
                        ))}
                      </li>
                    </ul>
                  </li>

                  <li
                    className={`nav-item dropdown ${
                      btmSecId === 2 ? NavbarStyle.btm_show : "dropdown"
                    }`}
                  >
                    <a
                      className={`nav-link dropdown-toggle ${
                        NavbarStyle.bottom_sec_dropdown
                      } ${
                        btmSecId === 2 ? NavbarStyle.btm_sec_list_active : ""
                      }`}
                      href="#"
                      role="button"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      onClick={() => toggleBottomDropdown(2)}
                    >
                      Attend & Learn
                    </a>
                    <ul
                      className={`dropdown-menu bg-E9F0F4 color-0053A5 ${
                        NavbarStyle.bottom_sec_dropdown_menu
                      } ${btmSecId === 2 ? NavbarStyle.btm_show : ""}`}
                    >
                      <li>
                        <a className="dropdown-item" href="#">
                          Action
                        </a>
                      </li>
                      <li>
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </li>

                  <li
                    className={`nav-item dropdown ${
                      btmSecId === 3 ? NavbarStyle.btm_show : "dropdown"
                    }`}
                  >
                    <a
                      className={`nav-link dropdown-toggle ${
                        NavbarStyle.bottom_sec_dropdown
                      } ${
                        btmSecId === 3
                          ? NavbarStyle.btm_sec_list_active
                          : "dropdown-toggle"
                      }`}
                      href="#"
                      role="button"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      onClick={() => toggleBottomDropdown(3)}
                    >
                      Power Your Business
                    </a>
                    <ul
                      className={`dropdown-menu bg-E9F0F4 color-0053A5 ${
                        NavbarStyle.bottom_sec_dropdown_menu
                      } ${btmSecId === 3 ? NavbarStyle.btm_show : ""}`}
                    >
                      <li>
                        <a className="dropdown-item" href="#">
                          Action
                        </a>
                      </li>
                      <li>
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </li>

                  <li
                    className={`nav-item dropdown ${
                      btmSecId === 4 ? NavbarStyle.btm_show : "dropdown"
                    }`}
                  >
                    <a
                      className={`nav-link dropdown-toggle ${
                        NavbarStyle.bottom_sec_dropdown
                      } ${
                        btmSecId === 4
                          ? NavbarStyle.btm_sec_list_active
                          : "dropdown-toggle"
                      }`}
                      href="#"
                      role="button"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      onClick={() => toggleBottomDropdown(4)}
                    >
                      Streaghten the Industry
                    </a>
                    <ul
                      className={`dropdown-menu bg-E9F0F4 color-0053A5 ${
                        NavbarStyle.bottom_sec_dropdown_menu
                      } ${btmSecId === 4 ? NavbarStyle.btm_show : ""}`}
                    >
                      <li>
                        <a className="dropdown-item" href="#">
                          Action
                        </a>
                      </li>
                      <li>
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </li>

                  <li
                    className={`nav-item dropdown ${
                      btmSecId === 5 ? NavbarStyle.btm_show : "dropdown"
                    }`}
                  >
                    <a
                      className={`nav-link dropdown-toggle ${
                        NavbarStyle.bottom_sec_dropdown
                      } ${
                        btmSecId === 5
                          ? NavbarStyle.btm_sec_list_active
                          : "dropdown-toggle"
                      }`}
                      href="#"
                      role="button"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      onClick={() => toggleBottomDropdown(5)}
                    >
                      Membership
                    </a>
                    <ul
                      className={`dropdown-menu bg-E9F0F4 color-0053A5 ${
                        NavbarStyle.bottom_sec_dropdown_menu
                      } ${btmSecId === 5 ? NavbarStyle.btm_show : ""}`}
                    >
                      <li>
                        <a className="dropdown-item" href="#">
                          Action
                        </a>
                      </li>
                      <li>
                        <a className="dropdown-item" href="#">
                          Another action
                        </a>
                      </li>
                    </ul>
                  </li>
                </ul>
                {/* End Menu on large screen */}
              </div>

              <ul
                className={`d-lg-none d-xl-none navbar-nav mb-2 ps-5 pt-3 mb-lg-0 text-start ${
                  responsiveToggleId > 0 ? "d-none" : "d-block"
                }`}
              >
                <li className="nav-item">
                  <a
                    className="color-FFFFFF fw-light nav-link active"
                    aria-current="page"
                    href="#"
                  >
                    NACS Show
                  </a>
                </li>
                <li className="nav-item">
                  <a className="color-FFFFFF fw-light nav-link" href="#">
                    Magazine
                  </a>
                </li>
                <li className="nav-item">
                  <a className="color-FFFFFF fw-light nav-link" href="#">
                    Store
                  </a>
                </li>
                <li className="nav-item">
                  <a className="color-FFFFFF fw-light nav-link" href="#">
                    About
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </nav>
      </div>
    </div>
  );
};
