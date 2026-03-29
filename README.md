# 🚀 Investor Radar

**Investor Radar** is an early-stage investment discovery platform designed to improve transparency in private investment opportunities by combining structured listings with SEO-driven visibility.

---

## 💡 Problem

Early-stage investment platforms often suffer from:

- lack of transparency in deal evolution (changes in terms are not tracked)
- low trust in listings (unclear if businesses or investors are legitimate)
- absence of structured validation (no expert or community-driven review)
- fragmented discovery across unreliable sources  

This project explores how a centralized platform could improve trust by combining structured listings with transparency and validation mechanisms.

---

## 🎯 Goal

The initial goal was to validate demand for a platform where:

- investors can discover structured investment opportunities  
- businesses can publish and manage investment listings  

Beyond the MVP, the product vision included building a **trust and transparency layer**, consisting of:

### 📜 Historical Transparency (planned)
- full change history for each listing  
- ability to track modifications (e.g. equity %, terms, updates over time)  

### ✅ Identity & Business Verification (planned)
- integration with external registries (e.g. YouControl)  
- potential integration with government services (e.g. Diia)  
- verification of:
  - business entities  
  - individual users  

### 💬 Community & Expert Validation (planned)
- comments system (implemented)  
- future extensions:
  - expert reviews (legal / financial analysis)  
  - validation scoring  
  - reputation system  

### 💰 Monetization Model (experimented)
- subscription-based access to premium insights  
- planned:
  - paid expert reviews  
  - revenue sharing for contributors (royalty model)  

👉 The MVP primarily focused on validating **traffic acquisition and monetization signals**, while the trust layer remained a planned extension.

---

## 📊 Results (MVP Validation)

- ~700 monthly visitors (organic traffic)
- ~1 minute average session duration  
- Ranked within **top 3 pages on Google** for query: *"інвестиції в Україні"*  
- ~6 months to achieve SEO visibility  

### ⚠️ Key Findings

- ~5000 registrations detected → likely bot traffic  
- Simulated subscription experiment (fake paywall):
  - users could unlock premium content via "Buy subscription" button  
  - **near-zero click-through rate**

👉 **Conclusion:**  
Traffic acquisition was successful, but **monetization demand was not validated** → project paused.

---

## 🧠 Key Engineering Decisions

### Monolith Architecture
- chosen for faster MVP delivery  
- reduced operational complexity  
- sufficient for current scale  

### Razor Pages (instead of SPA)
- tight integration with .NET  
- faster development  
- lower frontend complexity  

**Trade-off:** limited flexibility vs SPA frameworks  

---

### MSSQL + Entity Framework
- selected due to hosting constraints (cost optimization)  
- ensured fast and stable setup  

**Trade-off:** less flexibility compared to NoSQL solutions  

---

## 🏗 Architecture Overview

Modular monolith with clear separation of concerns:

- **UI Layer** – Razor Pages  
- **Application Layer** – business logic  
- **Domain Layer** – core entities  
- **Infrastructure Layer** – middleware & integrations  

---

## 🧩 RadarLibs (Reusable Modules)

Project includes reusable modules designed for extensibility:

### Radar.Domain
- core entities (posts, users, tags, comments)  
- investment listing structure  
- translation system  

### Radar.Application
- SEO generation (titles, descriptions)  
- slug generation  
- sitemap builder  
- validation logic  

### Radar.Infrastructure
- middleware:
  - SEO middleware  
  - language redirect  
  - WWW redirect  
- background jobs (Google Analytics sync)  
- authentication filters  

### Radar.UI
- reusable Razor components  

---

## ⚙️ Background Jobs

- Cron-based job (runs daily)
- Syncs Google Analytics data
- Updates view counts per listing  

**Limitations:**
- no retry mechanism  
- no failure monitoring
- A few credentials that you would find in app settings, it's just test account, so it doesn't matter for security policies as they were not used in production and were replaced during deployment.

---

## ⚡ Performance & Optimization

- query-level caching for expensive operations  
- layered architecture for maintainability  

---

## 🔐 Security

- minimal implementation (MVP stage)  
- fraud prevention & moderation → **planned but not implemented**

---

## 🚀 Deployment

- Hosted on **SmarterASP.NET**
- Optimized for low-cost MVP validation  

---

## 🧠 What This Project Demonstrates

- end-to-end MVP development  
- product validation mindset (not just coding)  
- pragmatic architectural decisions  
- SEO-driven backend design  
- ability to detect misleading metrics (bot traffic)  

---

## 📉 Why the Project Was Paused

- traffic acquisition worked (SEO)  
- engagement existed  
- **no monetization signal**

👉 Product hypothesis partially validated, business model not validated.

---

## 📌 Future Improvements

- anti-bot protection (CAPTCHA, rate limiting)  
- moderation system  
- real payment integration  
- API + SPA frontend separation  
- observability (logging, metrics, tracing)  

---

## ⚙️ Tech Stack

- **Backend:** C# / .NET Core  
- **Frontend:** Razor Pages  
- **Database:** MSSQL  
- **ORM:** Entity Framework  
- **Analytics:** Google Analytics  
- **Hosting:** SmarterASP.NET  

---

## ▶️ Getting Started

```bash
# clone repository
git clone https://github.com/your-username/investor-radar.git

# navigate to project
cd investor-radar

# run project
dotnet run
