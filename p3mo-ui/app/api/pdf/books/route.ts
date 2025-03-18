import { NextRequest, NextResponse } from 'next/server';

const API_URL = process.env.API_URL || 'http://localhost:5078/api';

export async function GET() {
  try {
    const response = await fetch(`${API_URL}/pdf/books`, {
      method: 'GET',
      headers: {
        'Accept': 'application/pdf'
      }
    });
    
    if (!response.ok) {
      throw new Error(`Error generating PDF: ${response.statusText}`);
    }
    
    const pdfBuffer = await response.arrayBuffer();
    
    return new NextResponse(pdfBuffer, {
      headers: {
        'Content-Type': 'application/pdf',
        'Content-Disposition': `attachment; filename="books-list.pdf"`
      }
    });
  } catch (error) {
    console.error('Error in PDF generation API route:', error);
    return NextResponse.json(
      { error: 'Failed to generate PDF' },
      { status: 500 }
    );
  }
}