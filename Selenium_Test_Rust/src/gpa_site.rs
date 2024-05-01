use thirtyfour::prelude::*;
use std::error::Error;

static URL: &'static str = "https://www.calculator.net/gpa-calculator.html";

pub async fn visit_and_reset(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    driver.goto(URL).await?;
    let clear_button = driver.find(By::Css("input[type='button'][value='Clear']")).await?;
    clear_button.click().await?;
    for i in 1..=3 {
        let grade_select = driver.find(By::Css(format!("select[name='la{i}'] > option"))).await?;
        grade_select.click().await?;
    }
    Ok(())
}
