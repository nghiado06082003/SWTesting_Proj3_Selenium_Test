use thirtyfour::prelude::*;
use std::error::Error;

pub async fn test_GPA_calculator(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    driver.goto("https://www.calculator.net/gpa-calculator.html").await?;
    Ok(())
}
