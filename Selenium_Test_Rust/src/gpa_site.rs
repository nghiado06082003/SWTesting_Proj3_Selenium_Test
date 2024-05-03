use core::time::Duration;
use thirtyfour::prelude::*;
use std::error::Error;

static URL: &'static str = "https://www.calculator.net/gpa-calculator.html";

pub async fn change_course_name(driver: &WebDriver, index: usize, name: &str) -> Result<(), Box<dyn Error + Send + Sync>> {
    let course_name_text_box = driver.find(By::Css(format!("input[type='text'][name='da{index}']"))).await?;
    course_name_text_box.clear().await?;
    course_name_text_box.send_keys(name).await?;
    Ok(())
}

pub async fn change_credits(driver: &WebDriver, index: usize, credits: isize) -> Result<(), Box<dyn Error + Send + Sync>> {
    let credits_text_box = driver.find(By::Css(format!("input[type='text'][name='ca{index}']"))).await?;
    credits_text_box.clear().await?;
    credits_text_box.send_keys(format!("{credits}")).await?;
    Ok(())
}

pub async fn change_grade(driver: &WebDriver, index: usize, grade: &str) -> Result<(), Box<dyn Error + Send + Sync>> {
    let grade_option = if grade != "-" {
        driver.find(By::Css(format!("select[name='la{index}'] > option[value='{}']", grade.to_lowercase()))).await?
    } else {
        driver.find(By::Css(format!("select[name='la{index}'] > option"))).await?
    };
    grade_option.click().await?;
    Ok(())
}

pub async fn click_clear(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    let clear_button = driver.find(By::Css("input[type='button'][value='Clear']")).await?;
    clear_button.click().await?;
    Ok(())
}

pub async fn click_calculate(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    let calculate_button = driver.find(By::Css("input[type='submit'][name='x'][value='Calculate']")).await?;
    calculate_button.click().await?;
    Ok(())
}

pub async fn get_result(driver: &WebDriver) -> Result<usize, Box<dyn Error + Send + Sync>> {
    let gpa_text = driver.find(By::Css("p.verybigtext > b")).await?.text().await?;
    Ok(gpa_text[5..].parse().expect("Unable to parse GPA string"))
}

pub async fn get_result_when_zero_credit(driver: &WebDriver) -> Result<usize, Box<dyn Error + Send + Sync>> {
    let gpa_text = driver.find(By::Css("h2.h2result + p")).await?.text().await?;
    Ok(gpa_text[15..].parse().expect("Unable to parse credit res string"))
}

pub async fn get_error_messages(driver: &WebDriver) -> Result<Vec<String>, Box<dyn Error + Send + Sync>> {
    let error_message_fonts = driver.find_all(By::Css("font[color='red']")).await?;
    let mut error_messages = vec![];
    for error in error_message_fonts {
        error_messages.push(error.text().await?);
    }
    Ok(error_messages)
}

pub async fn reset(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    click_clear(driver).await?;
    for i in 1..=3 {
        change_grade(driver, i, "-").await?;            
    }
    Ok(())
}

pub async fn visit(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    driver.goto(URL).await?;
    Ok(())
}

pub async fn visit_and_reset(driver: &WebDriver) -> Result<(), Box<dyn Error + Send + Sync>> {
    visit(driver).await?;
    tokio::time::sleep(Duration::new(1, 0)).await;
    reset(driver).await?;
    Ok(())
}


