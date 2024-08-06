<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

**Table of Contents** 

- [Summary](#summary)
- [Analysis problem and think for solution](#analysis-problem-and-think-for-solution)
  - [Problem](#problem)
  - [Analysis](#analysis)
  - [My solution:](#my-solution)
- [Architecture](#architecture)
  - [Database diagram](#database-diagram)
  - [System Architecture](#system-architecture)
- [Technologies:](#technologies)
  - [Front-End:](#front-end)
  - [Back-End:](#back-end)
- [Task progress](#task-progress)
- [Run Application](#run-application)
  - [Play with Microsoft Visual Studio SDK](#play-with-microsoft-visual-studio-sdk)
  - [Play with our live website](#play-with-our-live-website)
- [End](#end)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Summary

This is an simple web application of patients managements.

# Analysis problem and think for solution

## Problem

https://docs.google.com/document/d/1mCDD8p3cwsm0-f2eaYmEZvbG8_habWmfu8ettW75Sak/edit

## Analysis

https://docs.google.com/document/d/1qS_FNeMV1ET0WMmXS-C8AspC77MiE0mUsBlE6t0WTFU/edit

## My solution:

- Users up to 100K and possibly more -> Replication and cache to minimize query time
- At the same time, it is also necessary to optimize queries -> Database Tuning approach is so necessary
- Security: Patient data must be kept confidential, only authorized persons can access sensitive information.
- The user interface must be friendly and easy to use for both medical staff and patients.
- Perform regular backups and have a recovery plan after incidents. Write a data backup script

# Architecture

## Database diagram
![Untitled](https://github.com/user-attachments/assets/cc99a717-4525-4db7-ae98-f5b7fc91a2c9)
## System Architecture
![image](https://github.com/user-attachments/assets/ba8a1459-fcf8-4705-a522-019d73d3e7af)
# Technologies:

---

## Front-End:

🔗Link test Front-End:
<p align="center">
  <img src="https://vi.wikipedia.org/wiki/React" alt="React" width="100"/>
  <img src="https://www.bairesdev.com/blog/what-is-redux-and-why-it-matters" alt="Redux" width="100"/>
  <img src="https://www.youtube.com/@MUI_hq" alt="MUI" width="100"/>
  <img src="https://vi.wikipedia.org/wiki/Bootstrap" alt="Bootstrap" width="100"/>
  <img src="https://vercel.com/" alt="Vercel" width="100"/>
</p>
- React
- Redux
- MUI
- Bootstrap
- Deploy & CI: Vercel

## Back-End:

🔗Link test api: https://hospital-api-f6c5exhyheg3abfd.southeastasia-01.azurewebsites.net/
- ASP.NET core 8.0
- Postgres and Entity Framework
- Redis Cache
- Deploy: Gg cloud, Azure
- CI: Github action

# Task progress

👨‍💻Front-End:

- [x] Design UX/UI

- [x] Develop HTML & CSS templates

- [x] Integrate APIs

- [x] Enhance search performance on pages

- [x] Implement security measures

💻Back-End:

- [x] Data migration

- [x] Seed data for 100K patients, 200K contacts, and 200K addresses as requested

- [x] Build APIs

- [x] Document APIs with Swagger

- [x] Deploy application

- [x] Implement automatic backup

- [x] Implement caching to enhance performance when querying 100K patients

# Run Application

## 1. Play with Microsoft Visual Studio SDK

B1: Clone the git project
B2: Click on the `PatientManagementApi.sln`
B3: Choose the profile for running you want
B4: Run and if the swagger doc appear, that is successful

## 2. Play with our live website

Click on the link

# End

---

Thanks for Novobi Company
